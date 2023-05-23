namespace SkyscannerWeatherApi.Models.Home
{
    public class GeneralVM
    {
        public List<LocalesVM> localesVMs { get; set; }
        public List<MarketVM> marketVMs { get; set; }
        public List<CurrencyVM> currencyVMs { get; set; }

        public string SelectedLocalesCode { get; set; }
        public string SelectedMarketsCode { get; set; }
        public string SelectedCurrenciesCode { get; set; }
    }
}
