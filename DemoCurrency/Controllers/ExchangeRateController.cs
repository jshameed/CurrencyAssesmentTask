using AutoMapper;
using DemoCurrency.Entities;
using DemoCurrency.Model;
using DemoCurrency.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Globalization;
using System.Reflection;

namespace DemoCurrency.Controllers
{
    [Route("api/rates")]
    [ApiController]
    public class ExchangeRateController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ICurrencyServices _currencyServices;
        private readonly IMapper _mapper;

        public ExchangeRateController(ICurrencyServices currencyServices, IMapper mapper, IMemoryCache memoryCache)
        {
            _currencyServices = currencyServices ??
            throw new ArgumentNullException(nameof(currencyServices));
            _mapper = mapper ??
           throw new ArgumentNullException(nameof(mapper));
            _memoryCache = memoryCache ??
          throw new ArgumentNullException(nameof(memoryCache));
        }

     
        [HttpGet("{baseCurrency}", Name = "GetRates")]
        public async Task<ActionResult<RateEntitties>> GetRates(string baseCurrency  )
        {
            if (!await validateCurrency(baseCurrency))
                return BadRequest($"{baseCurrency} Currency is not valid");

            var rateEntitties = await _currencyServices.GetLatestRates(baseCurrency);
          //  Model.CurrencyRates currencyRates = _mapper.Map<Model.CurrencyRates>(rateEntitties);
            return Ok(rateEntitties);
        }

    
        [HttpGet("convert/{amount}/{fromCurrency}/{toCurrency}", Name = "ConvertRates")]
        public async Task<ActionResult<CurrencyRates>> ConvertRates(double amount, string fromCurrency, string toCurrency)
        {
            if (!await validateCurrency(fromCurrency))
                return BadRequest($"{fromCurrency} Currency is not valid");
            if (!await validateCurrency(toCurrency))
                return BadRequest($"{toCurrency} Currency is not valid");

            var rateEntitties = await _currencyServices.ConvertCurrency(fromCurrency, amount, toCurrency);
            Model.CurrencyRates currencyRates = _mapper.Map<Model.CurrencyRates>(rateEntitties);
            return Ok(currencyRates);

        }

        [HttpGet("history/{baseCurrency}/{startDate}/{endDate}/{pageSize}/{pageNumber}", Name = "GetRateHistory")]
        [ResponseCache(Duration = 600, VaryByQueryKeys = new[] { "baseCurrency", "startDate", "endDate", "pageSize", "pageNumber" })]
        public async Task<ActionResult<RateHistoryEntitiy>> GetRateHistory(string baseCurrency, string startDate, string endDate, int pageSize, int pageNumber)
        {
            if (!await validateCurrency(baseCurrency))
               return BadRequest($"{baseCurrency} Currency is not valid");

            var rateEntitties = await _currencyServices.GetRatesHistory(baseCurrency, startDate, endDate, pageSize, pageNumber);
           // Model.CurrencyRates currencyRates = _mapper.Map<Model.CurrencyRates>(rateEntitties);
            return Ok(rateEntitties);
        }

        private async Task<bool> validateCurrency(string currency)
        {
            string cacheKey = "currencies_key";
            if (!_memoryCache.TryGetValue(cacheKey, out Dictionary<string,string> currencyList))
            {
                currencyList = await _currencyServices.GetCurrencies();
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                    Priority = CacheItemPriority.High
                };
                // Save data in cache
                _memoryCache.Set(cacheKey, currencyList, cacheEntryOptions);
            }
            if (currencyList != null)
            {
                if (!currencyList.ContainsKey(currency))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
