namespace SkyscannerWeatherApi.Models.Flights
{
    public class AutosuggestFlightVM
    {
        public string Name { get; set; } 
        public string IataCode { get; set; }
        public string EntityID { get; set; }

        public AutosuggestFlightVM() { }
        public AutosuggestFlightVM(string name, string iataCode, string entityID)
        {
            Name = name;
            IataCode = iataCode;
            EntityID = entityID;
        }
    }
}
