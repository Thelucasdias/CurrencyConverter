using Newtonsoft.Json;


namespace CurrencyConverter.Services
{
    public class CurrencyService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public CurrencyService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["FreeCurrencyApi:ApiKey"]
                  ?? throw new ArgumentNullException(nameof(configuration), "Chave de API não encontrada.");
        }
         public async Task<List<string>> GetCurrencies()
        {
            var url = $"https://api.freecurrencyapi.com/v1/currencies?apikey={_apiKey}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var currenciesResponse = JsonConvert.DeserializeObject<CurrencyListResponse>(content);

            if (currenciesResponse?.Data == null)
            {
                throw new Exception("Resposta inválida da API.");
            }

            var currencies = currenciesResponse.Data.Keys.ToList();
            return currencies;}

        public async Task<decimal> ConvertCurrency(string from, string to, decimal amount)
        {
            var url = $"https://api.freecurrencyapi.com/v1/latest?base_currency={from}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Erro ao acessar API: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<FreeCurrencyApiResponse>(content);

            if (result?.Data != null && result.Data.ContainsKey(to))
            {
                return result.Data[to] * amount;
            }

            throw new Exception("Moeda de destino não encontrada ou resposta inválida da API.");
        }

            public class CurrencyConversionResponse
        {
            public Dictionary<string, decimal> Data { get; set; } = new Dictionary<string, decimal>();
        }

    
        public class FreeCurrencyApiResponse
    {
        public required Dictionary<string, decimal> Data { get; set; }
    }
    public class CurrencyInfo
    {
        public required string Name { get; set; }
        public required string Symbol { get; set; }
    }

    internal class CurrencyListResponse
    {
        public required Dictionary<string, CurrencyInfo> Data { get; set; }
    }
}
}

    