using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using System.Configuration;
using Newtonsoft.Json;
using SkyscannerWeatherApi.Models.Flights;
using SkyscannerWeatherApi.RequestMethods;
using SkyscannerWeatherApi.Models.Home;
using System.Numerics;

namespace SkyscannerWeatherApi.Controllers
{
    [Route("api/[controller]")]
    
    public class FlightsController : Controller
    {
        public static Dictionary<string, string> dictionary = new Dictionary<string, string>();
        public static List<ResultFlightVM> list = new List<ResultFlightVM>();
        private readonly IRequestSender requestSender;
        private static string locale = "bg-BG";
        private static string market = "BG";
        private static string currency = "BGN";

        public FlightsController(IRequestSender _requestSender)
        {
            requestSender = _requestSender;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string localeCode, string marketCode, string currencyCode)
        
        {
            var url = "https://partners.api.skyscanner.net/apiservices/v3/autosuggest/flights";
            
            //set values to the global variables, which we will use in every query 
            locale = localeCode;
            market = marketCode;
            currency = currencyCode;

            SearchVM model = new SearchVM();

            JToken token = await requestSender.SendAutosuggestRequest(url, locale, market, currency);
            if (!token.Equals("Unsuccessful"))
            {
                requestSender.FillAutosuggestList(token, model);
                return View(model);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(SearchVM model)
        {
            var url = "https://partners.api.skyscanner.net/apiservices/v3/flights/live/search/create";
            
            ResultVM resultModel = await requestSender.SendSearchFlightRequest(url,  locale,  market,  currency,  model);


            list = resultModel.resultFlightVMs;
            // Redirect to the GET action with the serialized view model as a query parameter
            return RedirectToAction("Result", "Flights");
            //return RedirectToAction("Result", "Flights", resultModel);
        }

        [HttpGet("Result")]
        public IActionResult Result()
        {
            
            foreach (var item in list)
            {
                if (dictionary.ContainsKey(item.OriginPlaceId))
                {
                    item.OriginPlaceId= dictionary[item.OriginPlaceId];
                }
                if (dictionary.ContainsKey(item.DestinationPlaceId))
                {
                    item.DestinationPlaceId = dictionary[item.DestinationPlaceId];
                }

            }
            ResultVM model = new ResultVM()
            {
                resultFlightVMs = list
            };
            return View(model);
        }



    }
}
