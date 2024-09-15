using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DemoCurrency.ErrorHandlers
{
    public class GenericValidationProblemDetails : ProblemDetails
    {
        public IDictionary<string, string[]> Errors { get; set; }

     
    }
}
