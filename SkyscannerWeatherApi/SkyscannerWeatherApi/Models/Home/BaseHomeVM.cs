namespace SkyscannerWeatherApi.Models.Home
{
    public class BaseHomeVM
    {
        public string Code { get; set; }
        public BaseHomeVM() { }
        public BaseHomeVM(string code) 
        {
            Code = code;
        }
    }
}
