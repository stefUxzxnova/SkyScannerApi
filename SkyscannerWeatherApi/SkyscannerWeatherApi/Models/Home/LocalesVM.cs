namespace SkyscannerWeatherApi.Models.Home
{
    public class LocalesVM : BaseHomeVM
    {
        
        public string Name { get; set; }

        public LocalesVM() :base() { }
        
        public LocalesVM(string code, string name) : base(code)
        {
            Name = name;
        }
    }
}
