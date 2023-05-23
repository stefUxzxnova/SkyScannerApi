namespace SkyscannerWeatherApi.Models.Flights
{
    public class ResultFlightVM
    {
        public string OriginPlaceId { get; set; }
        public string DestinationPlaceId { get; set; }
        public string ItineraryId { get; set; }
        public int StopCount { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public List<string> Agents { get; set; }
        public decimal Price { get; set; }
        public int DurationInMinutes { get; set; }
        public double CheapestScore { get; set; }
        public string DeepLink { get; set; }
    }
}
