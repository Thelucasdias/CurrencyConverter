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

        public async Task<decimal> ConvertCurrency(string from, string to, decimal amount)
        {
            var url = $"https://api.freecurrencyapi.com/v1/latest?apikey={_apiKey}&base_currency={from}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<FreeCurrencyApiResponse>(content);

            if (result?.Data != null && result.Data.ContainsKey(to))
            {
                return result.Data[to] * amount;
            }

            throw new Exception("Moeda de destino não encontrada ou resposta inválida da API.");
        }
    }

    public class FreeCurrencyApiResponse
    {
        public required Dictionary<string, decimal> Data { get; set; }
    }
}
