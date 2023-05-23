namespace SkyscannerWeatherApi.Models.Home
{
    public class MarketVM : BaseHomeVM
    {
        public string Name { get; set; }

        public MarketVM() : base()
        { }
        public MarketVM(string code, string name) : base(code)
        {
            Name = name;
            
        }
    }
}
