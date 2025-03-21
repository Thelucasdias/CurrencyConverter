using Microsoft.AspNetCore.Mvc;
using CurrencyConverter.Services;

namespace CurrencyConverter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly CurrencyService _currencyService;

        public CurrencyController(CurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        
        [HttpGet("currencies")]
        public async Task<IActionResult> GetCurrencies()
        {
            try
            {
                var currencies = await _currencyService.GetCurrencies();
                return Ok(currencies);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [HttpGet("convert")]
        public async Task<IActionResult> Convert(string from, string to, decimal amount)
        {
            try
            {
                var result = await _currencyService.ConvertCurrency(from, to, amount);
                var formattedResult = result.ToString("F2");
                return Ok(formattedResult);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}