async function loadCurrencies() {
  try {
    const response = await fetch(
      "http://localhost:5298/api/currency/currencies"
    );
    if (!response.ok) {
      throw new Error("Erro ao carregar moedas");
    }

    const currencies = await response.json();
    const fromSelect = document.getElementById("from");
    const toSelect = document.getElementById("to");

    currencies.forEach((currency) => {
      const option = document.createElement("option");
      option.value = currency;
      option.textContent = currency;
      fromSelect.appendChild(option);

      const option2 = document.createElement("option");
      option2.value = currency;
      option2.textContent = currency;
      toSelect.appendChild(option2);
    });
  } catch (error) {
    console.error("Erro:", error);
  }
}

async function convertCurrency(event) {
  event.preventDefault();

  const from = document.getElementById("from").value;
  const to = document.getElementById("to").value;
  const amount = document.getElementById("amount").value;

  try {
    const response = await fetch(
      `http://localhost:5298/api/currency/convert?from=${from}&to=${to}&amount=${amount}`
    );
    if (!response.ok) {
      throw new Error("Erro na requisição");
    }

    const result = await response.text();
    document.getElementById(
      "result"
    ).innerText = `$${amount} ${from} = $${result}${to}`;
  } catch (error) {
    document.getElementById("result").innerText = `Erro: ${error.message}`;
  }
}

document.addEventListener("DOMContentLoaded", loadCurrencies);

document
  .getElementById("converterForm")
  .addEventListener("submit", convertCurrency);

// Função para buscar dados da API
async function fetchMarketData() {
  try {
    const response = await fetch(
      "https://api.infomoney.com.br/ativos/ticker?type=json&_locale=user"
    );

    if (!response.ok) {
      throw new Error("Erro na resposta da API");
    }

    return await response.json();
  } catch (error) {
    console.error("Erro ao buscar dados:", error);
    return null;
  }
}

// Função para renderizar os itens (sem bandeiras)
function renderTickerItems(data) {
  const container = document.querySelector(".ticker-container");
  container.innerHTML = ""; // Limpa o container

  data.forEach((item) => {
    const itemElement = document.createElement("div");
    itemElement.className = "ticker-item";

    itemElement.innerHTML = `
      <span class="ticker-name">${item.Name}</span>
      <span class="ticker-value">${item.Value}</span>
      <span class="ticker-variation ${item.Direction}">${item.Spread}</span>
    `;

    container.appendChild(itemElement);
  });
}

// Função principal
async function initTicker() {
  // Mostra loading
  document.querySelector(".ticker-container").innerHTML =
    '<div class="ticker-item">Carregando...</div>';

  const marketData = await fetchMarketData();

  if (marketData && marketData.length) {
    renderTickerItems(marketData);

    // Atualiza a cada 1 minuto
    setInterval(async () => {
      const freshData = await fetchMarketData();
      if (freshData) renderTickerItems(freshData);
    }, 60000);
  } else {
    document.querySelector(".ticker-container").innerHTML =
      '<div class="ticker-item">Erro ao carregar dados. Atualize a página.</div>';
  }
}

// Inicia quando o DOM estiver pronto
document.addEventListener("DOMContentLoaded", initTicker);
