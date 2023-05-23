using Newtonsoft.Json.Linq;
using SkyscannerWeatherApi.Models.Flights;
using SkyscannerWeatherApi.Models.Home;
using System;
using System.Reflection;

namespace SkyscannerWeatherApi.RequestMethods
{
    public interface IRequestSender
    {
        Task<JToken> SendAutosuggestRequest(string url, string localeCode, string marketCode, string currencyCode);
        Task<JToken> SendRequest(string url, string dataNode);
        Task<ResultVM> SendSearchFlightRequest(string url, string locale, string market, string currency, SearchVM model);

        public void FillLocalesList(JToken token, GeneralVM model);
        public void FillMarketsList(JToken token, GeneralVM model);
        public void FillCurrenciesList(JToken token, GeneralVM model);
        public void FillAutosuggestList(JToken token, SearchVM model);
        

    }
}
