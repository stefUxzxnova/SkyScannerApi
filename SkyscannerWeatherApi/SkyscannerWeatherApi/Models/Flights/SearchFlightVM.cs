namespace SkyscannerWeatherApi.Models.Flights
{
    public class SearchFlightVM
    {
        public string IataCodeOriginPlace { get; set; }
        public string IataCodeDastinationPlace { get; set; }
        public int Adults { get; set; } 
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
    }
}
