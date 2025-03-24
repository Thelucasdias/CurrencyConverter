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
