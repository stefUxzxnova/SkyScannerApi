using Newtonsoft.Json.Linq;

namespace SkyscannerWeatherApi.Models.Locales
{
    public class GetVM
    {
        public List<JToken> Locales { get; set; }
        public GetVM()
        {
            Locales = new List<JToken>();
        }
    }
}
