using System.Reflection;

namespace DemoCurrency
{

    [AttributeUsage(AttributeTargets.Method)]
    public class SkipDuringTestAttribute : Attribute { }
    
}
