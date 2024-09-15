using Newtonsoft.Json;
using System.Globalization;

namespace DemoCurrency.Model
{
    public class CurrencyRates
    {
        public string Amount { get; set; }
        public string Base { get; set; }
        public string Date { get; set; }
        public Dictionary<string, string> Rates { get; set; }


        public CurrencyRates()
        {
           
            
        }

    }
}
