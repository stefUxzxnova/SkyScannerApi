namespace SkyscannerWeatherApi.Models.Flights
{
    public class ResultVM
    {
        public List<ResultFlightVM> resultFlightVMs { get; set; }
        
        public ResultVM() 
        {
            resultFlightVMs= new List<ResultFlightVM>();
        }
    }
}
