using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SkyscannerWeatherApi.Controllers;
using SkyscannerWeatherApi.Models.Flights;
using SkyscannerWeatherApi.Models.Home;
using System.Text;

namespace SkyscannerWeatherApi.RequestMethods
{
    public class RequestSender : IRequestSender
    {
        private readonly HttpClient client;

        public RequestSender(HttpClient client)
        {
            this.client = client;
            this.client.DefaultRequestHeaders.Add("x-api-key", "prtl6749387986743898559646983194");

        }

        public async Task<JToken> SendRequest(string url, string dataNode)
        {
            //take the Status code
            var response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var body = response.Content.ReadAsStringAsync().Result;
                JObject data = JObject.Parse(body);

                //we take child nodes of "locales"
                JToken token = data.GetValue($"{dataNode}");

                // Check if the "locales" property exists and is an array
                if (token != null && token.Type == JTokenType.Array)
                {
                    return token;
                }

            }
            return "Unsuccessful";

        }
        public async Task<JToken> SendAutosuggestRequest(string url, string localeCode, string marketCode, string currencyCode)
        {
            //Request body (json)
            //Anonymous type named body with two properties: query and limit.
            //The query property is also an anonymous type with three properties:
            //market, locale, and searchTerm
            var body = new
            {
                query = new
                {
                    market = $"{marketCode}",
                    locale = $"{localeCode}",
                    currency = $"{currencyCode}"

                },
                limit = 50
            };
            //serialize the body->to json
            var json = JsonConvert.SerializeObject(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            //take the Status code
            var response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseData = response.Content.ReadAsStringAsync().Result;
                JObject data = JObject.Parse(responseData);

                JToken token = data.GetValue("places");

                if (token != null && token.Type == JTokenType.Array)
                {
                    return token;
                }

            }
            return "Unsuccessful";
        }

        public async Task<ResultVM> SendSearchFlightRequest(string url, string locale, string market, string currency, SearchVM model)
        {
            ResultVM resultVM = new ResultVM();
            //serialize the body->to json
            var requestBody = new
            {
                query = new
                {
                    market = $"{market}",
                    locale = $"{locale}",
                    currency = $"{currency}",
                    queryLegs = new[]
                    {
                        new
                        {
                            originPlaceId = new
                            {
                                iata = $"{model.SelectedDepartureCode}"
                            },
                            destinationPlaceId = new
                            {
                                iata = $"{model.SelectedDestinationCode}"
                            },
                            date = new
                            {
                                year = model.DepartureDate.Year,
                                month = model.DepartureDate.Month,
                                day = model.DepartureDate.Day
                            }
                        }
                    },
                    adults = model.Adults,
                    cabinClass = "CABIN_CLASS_ECONOMY"
                }
            };
            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            //take the Status code
            var response = await client.PostAsync(url, content);


            if (response.IsSuccessStatusCode)
            {
                var responseData = response.Content.ReadAsStringAsync().Result;
                JObject data = JObject.Parse(responseData);

                // Access the itineraries object
                JObject itineraries = (JObject)data["content"]["results"]["itineraries"];

                // Iterate over each itinerary
                foreach (var itinerary in itineraries)
                {
                    ResultFlightVM flight = new ResultFlightVM();
                    flight.ItineraryId = itinerary.Key;
                    JObject itineraryData = (JObject)itinerary.Value;

                    // Access the pricingOptions array for the current itinerary
                    JArray pricingOptions = (JArray)itineraryData["pricingOptions"];

                    // Iterate over each pricing option
                    foreach (JObject option in pricingOptions)
                    {
                        // Access price, agent IDs, and items for the current option
                        JObject price = (JObject)option["price"];
                        JArray agentIds = (JArray)option["agentIds"];
                        JArray items = (JArray)option["items"];

                        // Extract values from price
                        flight.Price = (decimal)price["amount"] / 1000;

                        // Extract values from agentIds
                        flight.Agents = agentIds.ToObject<List<string>>();

                        // Extract values from items
                        JObject item = (JObject)items.FirstOrDefault();

                        JObject itemPrice = (JObject)item["price"];

                        flight.DeepLink = (string)item["deepLink"];
                    }

                    JObject legs = (JObject)data["content"]["results"]["legs"];
                    foreach (var leg in legs)
                    {
                        string legKey = leg.Key;
                        JObject legData = (JObject)leg.Value;

                        // Compare the leg key with your flight object's key
                        if (legKey == flight.ItineraryId)
                        {
                            // Extract leg information
                            flight.OriginPlaceId = (string)legData["originPlaceId"];
                            flight.DestinationPlaceId = (string)legData["destinationPlaceId"];

                            JObject departureDateTime = (JObject)legData["departureDateTime"];
                            int departureYear = (int)departureDateTime["year"];
                            int departureMonth = (int)departureDateTime["month"];
                            int departureDay = (int)departureDateTime["day"];
                            int departureHour = (int)departureDateTime["hour"];
                            int departureMinute = (int)departureDateTime["minute"];
                            int departureSecond = (int)departureDateTime["second"];

                            flight.DepartureDate = new DateTime(departureYear, departureMonth, departureDay, departureHour, departureMinute, departureSecond);


                            JObject arrivalDateTime = (JObject)legData["arrivalDateTime"];
                            int arrivalYear = (int)arrivalDateTime["year"];
                            int arrivalMonth = (int)arrivalDateTime["month"];
                            int arrivalDay = (int)arrivalDateTime["day"];
                            int arrivalHour = (int)arrivalDateTime["hour"];
                            int arrivalMinute = (int)arrivalDateTime["minute"];
                            int arrivalSecond = (int)arrivalDateTime["second"];

                            flight.ArrivalDate = new DateTime(departureYear, departureMonth, departureDay, departureHour, departureMinute, departureSecond);

                            flight.DurationInMinutes = (int)legData["durationInMinutes"];
                            flight.StopCount = (int)legData["stopCount"];
                            break;
                        }
                        // Process the extracted leg information as needed

                    }

                    resultVM.resultFlightVMs.Add(flight);

                }
            }
            return resultVM;
        }

        public void FillLocalesList(JToken token, GeneralVM model)
        {
            model.localesVMs = new List<LocalesVM>();
            for (int i = 1; i < token.Count(); i++)
            {
                var item = new LocalesVM()
                {
                    Code = (string)token.ElementAt(i).Value<string>("code"),
                    Name = (string)token.ElementAt(i).Value<string>("name")
                };
                model.localesVMs.Add(item);
            }
        }

        public void FillMarketsList(JToken token, GeneralVM model)
        {
            model.marketVMs = new List<MarketVM>();
            for (int i = 1; i < token.Count(); i++)
            {
                var item = new MarketVM()
                {
                    Code = (string)token.ElementAt(i).Value<string>("code"),
                    Name = (string)token.ElementAt(i).Value<string>("name"),
                };
                model.marketVMs.Add(item);
            }
        }

        public void FillCurrenciesList(JToken token, GeneralVM model)
        {
            model.currencyVMs = new List<CurrencyVM>();
            for (int i = 1; i < token.Count(); i++)
            {
                var item = new CurrencyVM()
                {
                    Code = (string)token.ElementAt(i).Value<string>("code"),
                    Symbol = (string)token.ElementAt(i).Value<string>("symbol"),
                };
                model.currencyVMs.Add(item);
            }
        }

        public void FillAutosuggestList(JToken token, SearchVM model)
        {
            FlightsController.dictionary.Clear();
            model.Flights = new List<AutosuggestFlightVM>();
            for (int i = 1; i < token.Count(); i++)
            {
                var item = new AutosuggestFlightVM()
                {
                    IataCode = token.ElementAt(i).Value<string>("iataCode"),
                    Name = token.ElementAt(i).Value<string>("name"),
                    EntityID = token.ElementAt(i).Value<string>("entityId"),
                };
                model.Flights.Add(item);
                FlightsController.dictionary.Add(item.EntityID, item.Name);
            }
        }

    }
}
