namespace CurrencyConverter.Models
{
    public class ExchangeRatesResponse
    {
        public required Dictionary<string, decimal> Rates { get; set; }
    }
}