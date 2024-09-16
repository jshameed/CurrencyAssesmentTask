using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace DemoCurrency.Filters
{
    public class BlockCurrencyActionFilter : IActionFilter
    {
        private readonly List<string> blockedCurrencyList = new List<string> { "TRY", "PLN", "THB", "MXN" };

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var path = context.HttpContext.Request.Path.ToString();
            var blockedCurrency = blockedCurrencyList.FirstOrDefault(blocked => path.Contains(blocked));

            if (blockedCurrency != null)
            {
                context.Result = new BadRequestObjectResult($"Blocked currency in parameter found.: {blockedCurrency}");
                return;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
