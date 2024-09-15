using DemoCurrency.Entities;
using DemoCurrency.Model;

namespace DemoCurrency.Services
{
    public class CurrencyServices : ICurrencyServices
    {
        private FrankfurterAPIClient _frankfurterapiclient;
        public CurrencyServices(FrankfurterAPIClient frankfurterapiclient)
        {
            _frankfurterapiclient = frankfurterapiclient;
        }

        public async Task<RateEntitties?> GetLatestRates(string basecurrency)
        {
            var rates = await _frankfurterapiclient.GetExchangeRatesAsync(basecurrency);
            return rates;
        }

        public async Task<RateEntitties?> ConvertCurrency(string fromCurrency, double amount, string toCurrency)
        {
            var result = await _frankfurterapiclient.ConvertCurrencyAsync(fromCurrency,amount,toCurrency);
            return result;
        }

        public async Task<RateHistoryEntitiy?> GetRatesHistory(string baseCurrency,  string startdate, string enddate, int pageSize, int pageNumber)
        {
            var result = await _frankfurterapiclient.GetRateHistoryAsync(baseCurrency, startdate, enddate, pageSize, pageNumber);
            return result;
        }

        public async Task<Dictionary<string,string>> GetCurrencies()
        {
            var result = await _frankfurterapiclient.GetCurrenciesAsync();
            return result;
        }
    }
}
