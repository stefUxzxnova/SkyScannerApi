using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SkyscannerWeatherApi.Models;
using SkyscannerWeatherApi.Models.Home;
using SkyscannerWeatherApi.RequestMethods;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using System.Text;

namespace SkyscannerWeatherApi.Controllers
{
    public class HomeController : Controller
    {
        //we need these for all queries
        private string localeCode;
        private string marketCode;
        private string currencyCode;

        private readonly IRequestSender requestSender;

        public HomeController(IRequestSender _requestSender)
        {
            requestSender = _requestSender;
        }
        public async Task<IActionResult> Index()
        {
            var urlLanguages = "https://partners.api.skyscanner.net/apiservices/v3/culture/locales";
            var urlMarkets = "https://partners.api.skyscanner.net/apiservices/v3/culture/markets/en-GB";
            var urlCurrencies = "https://partners.api.skyscanner.net/apiservices/v3/culture/currencies";

            GeneralVM generalVM = new GeneralVM();
            //generalVM.localesVMs = new List<LocalesVM>();
            //generalVM.marketVMs = new List<MarketVM>();
            //generalVM.currencyVMs = new List<CurrencyVM>();

            
            JToken token =  await requestSender.SendRequest(urlLanguages, "locales");
            if (!token.Equals("Unsuccessful"))
            {
                 requestSender.FillLocalesList(token, generalVM);
            }
            token = await requestSender.SendRequest(urlMarkets, "markets");
            if (!token.Equals("Unsuccessful"))
            {
                requestSender.FillMarketsList(token, generalVM);
            }
            token = await requestSender.SendRequest(urlCurrencies, "currencies");
            if (!token.Equals("Unsuccessful"))
            {
                requestSender.FillCurrenciesList(token, generalVM);
            }

            return View(generalVM);
        }

        [HttpPost]
        public IActionResult SaveChanges(GeneralVM model)
        {
            localeCode = model.SelectedLocalesCode;
            marketCode = model.SelectedMarketsCode;
            currencyCode = model.SelectedCurrenciesCode;
            if (localeCode == null || marketCode == null || currencyCode == null)
            {
                return RedirectToAction("Index", "Home");
            }

            //HttpContext.Session.SetString("LocaleCode", localeCode);
            //HttpContext.Session.SetString("MarketCode", marketCode);
            //HttpContext.Session.SetString("CurrencyCode", currencyCode);


            return RedirectToAction("Index", "Flights", new
            {
                LocaleCode = localeCode,
                MarketCode = marketCode,
                CurrencyCode = currencyCode
            });
           
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}