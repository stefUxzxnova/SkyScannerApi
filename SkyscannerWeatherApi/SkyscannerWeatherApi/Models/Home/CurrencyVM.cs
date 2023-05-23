namespace SkyscannerWeatherApi.Models.Home
{
    public class CurrencyVM : BaseHomeVM
    {
        public string Symbol { get; set; }
        public CurrencyVM() : base()
        { }

        public CurrencyVM(string code, string symbol) : base(code) 
        {
            Symbol = symbol;
        }
    }
}
