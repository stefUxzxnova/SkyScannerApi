namespace SkyscannerWeatherApi.Models.Flights
{
    public class SearchVM
    {
        public List<AutosuggestFlightVM> Flights { get; set; }

        public string SelectedDepartureCode { get; set; }
        public string SelectedDestinationCode { get; set; }
        public int Adults { get; set; }
        public DateTime DepartureDate { get; set; }
        public string Cabin { get; set;}
        
    }
}
