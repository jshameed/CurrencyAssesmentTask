
using DemoCurrency.Entities;
using DemoCurrency.ErrorHandlers;
using DemoCurrency.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
namespace DemoCurrency
{
    public class FrankfurterAPIClient
    {
        private readonly ILogger<FrankfurterAPIClient> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptionsHelper _jsonSerializerOptionsHelper;

        public FrankfurterAPIClient(IHttpClientFactory httpClientFactory,
        JsonSerializerOptionsHelper jsonSerializerOptionsHelper, ILogger<FrankfurterAPIClient> logger)
        {
            _httpClientFactory = httpClientFactory ??
                throw new ArgumentNullException(nameof(httpClientFactory));
            _logger = logger ??
               throw new ArgumentNullException(nameof(logger));
            _jsonSerializerOptionsHelper = jsonSerializerOptionsHelper ??
                throw new ArgumentNullException(nameof(jsonSerializerOptionsHelper));
        }

        public async Task<RateEntitties?> GetExchangeRatesAsync(string basecurrency)
        {
            var query = $"\\latest?from={basecurrency}";
            var content = await ProcessGetRequest(query);
            return JsonSerializer.Deserialize<RateEntitties>(content, _jsonSerializerOptionsHelper.Options);
        }

        public async Task<Dictionary<string, string>?> GetCurrenciesAsync()
        {
            var query = $"/currencies";
            var content = await ProcessGetRequest(query);
            return JsonSerializer.Deserialize<Dictionary<string, string>>(content,_jsonSerializerOptionsHelper.Options);
        }

        public async Task<RateEntitties?> ConvertCurrencyAsync(string fromCurrency, double amount, string toCurrency)
        {
            var query = $"\\latest?amount={amount}&from={fromCurrency}&to={toCurrency}";
            var content = await ProcessGetRequest(query);
            return JsonSerializer.Deserialize<RateEntitties>(content, _jsonSerializerOptionsHelper.Options);
        }

        public async Task<RateHistoryEntitiy?> GetRateHistoryAsync(string baseCurrency, string startdate, 
                                                string enddate, int pagesize, int pageNumber)
        {
            const int MAXPAGESIZE = 100;
            pagesize = (pagesize > MAXPAGESIZE) ? MAXPAGESIZE : pagesize;
            startdate = DateTime.Parse(startdate).AddDays(pagesize * (pageNumber-1)).ToString("yyyy-MM-dd");
            string enddate2 = DateTime.Parse(startdate).AddDays(pagesize-1).ToString("yyyy-MM-dd");
            enddate = DateTime.Parse(enddate2) > DateTime.Parse(enddate) ? enddate : enddate2;

            var query = $"\\{startdate}..{enddate}?from={baseCurrency}";
            var content = await ProcessGetRequest(query);
            return JsonSerializer.Deserialize<RateHistoryEntitiy>(content, _jsonSerializerOptionsHelper.Options);

        }

        public async Task<string> ProcessGetRequest(string query)
        {
            var httpClient = _httpClientFactory.CreateClient("FrankfurterClient");
            var request = new HttpRequestMessage(HttpMethod.Get, query);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                if (!response.IsSuccessStatusCode)
                    await LogClientError(response);

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
                
            }
        }

        public async Task LogClientError(HttpResponseMessage response)
        {
            var errorStream = await response.Content.ReadAsStreamAsync();

            var errorAsProblemDetails = await JsonSerializer.DeserializeAsync<ValidationProblemDetails>(
            errorStream,
            _jsonSerializerOptionsHelper.Options);

            var errors = errorAsProblemDetails?.Errors;
            Console.WriteLine(errorAsProblemDetails?.Title);
            _logger.LogError(errorAsProblemDetails?.Title);
        }
    }
}
