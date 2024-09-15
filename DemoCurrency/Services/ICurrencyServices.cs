using DemoCurrency.Entities;
using DemoCurrency.Model;

namespace DemoCurrency.Services
{
    public interface ICurrencyServices
    {
         Task<RateEntitties?> GetLatestRates(string basecurrency);
        Task<RateEntitties?> ConvertCurrency(string fromCurrency, double amount, string toCurrency);
        Task<RateHistoryEntitiy?> GetRatesHistory(string baseCurrency, string startdate,string enddate, int pageSize, int pageNumber);
        Task<Dictionary<string, string>> GetCurrencies();

    }

}
