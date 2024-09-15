using System.Text.Json;
using DemoCurrency.Entities;
using DemoCurrency.Services;
using Xunit.Sdk;
using Moq;
using DemoCurrency.Controllers;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using DemoCurrency.Model;

namespace DemoCurrency.Test
{
    public class LatestRatesTest
    {

        private readonly ExchangeRateController _currencyController;
        private readonly Mock<ICurrencyServices> _mockCurrencyService;
        private readonly Mock<IMemoryCache> _mockmemoryCache;
        private readonly Mock<IMapper> _mockmapper;
      
        public LatestRatesTest()
        {
            _mockCurrencyService = new Mock<ICurrencyServices>();
            _mockmemoryCache = new Mock<IMemoryCache>();
            _mockmapper = new Mock<IMapper>();
            _currencyController = new ExchangeRateController(_mockCurrencyService.Object, _mockmapper.Object, _mockmemoryCache.Object);
        }


        [Fact]
        public async Task GetLatestRates_BaseCurrency_EUR()
        {
            using var streamCurrency = await TestData.GetStreamAsync("Currencies.json");
            var actualCurrencies = await JsonSerializer.DeserializeAsync<Dictionary<string,string>>(streamCurrency);

            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            using var stream = await TestData.GetStreamAsync("Rates.json");
            var actualRates = await JsonSerializer.DeserializeAsync<RateEntitties>(stream, options);

            object cacheEntry;
            _mockmemoryCache.Setup(mc => mc.TryGetValue(It.IsAny<object>(), out cacheEntry))
                .Returns(true)
                .Callback((object key, out object value) =>
                {
                    value = actualCurrencies;
                });


           _mockCurrencyService.Setup(s => s.GetLatestRates("EUR")).ReturnsAsync(actualRates);
            var result =  await _currencyController.GetRates("EUR");

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var expectedResult = Assert.IsType<RateEntitties>(okResult.Value);
            Assert.Equal(30, expectedResult.Rates.Count);
            Assert.Equal("EUR", expectedResult.Base);
            Assert.Equal("2024-09-13", expectedResult.Date);
            Assert.Equal(1.0, expectedResult.Amount);
        }
    }
}