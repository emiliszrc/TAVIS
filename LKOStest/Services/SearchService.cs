using LKOStest.Dtos;
using LKOStest.Exceptions;
using LKOStest.Interfaces;
using Newtonsoft.Json;
using RestSharp;

namespace LKOStest.Services
{
    public class SearchService : ISearchService
    {
        public SearchResponse SearchForLocation(string location)
        {
            var client = new RestClient($"https://tripadvisor1.p.rapidapi.com/locations/search?location_id=1&limit=30&sort=relevance&offset=0&lang=en_US&currency=USD&units=km&");
            var request = new RestRequest(Method.GET);
            request.AddQueryParameter("query", location);
            AddRapidApiHeaders(request);

            var response = client.Execute(request);

            if (!response.StatusCode.Equals(System.Net.HttpStatusCode.OK))
            {
                throw new TripAdvisorApiException($"Could not retrieve information from TripAdvisor. Response code: {response.StatusCode}");
            }

            var locationSearchResponse = JsonConvert.DeserializeObject<SearchResponse>(response.Content);

            return locationSearchResponse;
        }

        private void AddRapidApiHeaders(RestRequest request)
        {
            request.AddHeader("x-rapidapi-host", "tripadvisor1.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "53055efebdmshe3835e4cfdf29c4p1b412bjsn58803bacff0c");
        }
    }
}
