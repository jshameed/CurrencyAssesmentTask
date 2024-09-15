using System.Text.Json;

namespace DemoCurrency.Helper
{
    public class JsonSerializerOptionsHelper
    {
        public JsonSerializerOptions Options { get; }
        public JsonSerializerOptionsHelper() {
            Options = new JsonSerializerOptions(
                JsonSerializerDefaults.Web);
        }
    }
}
