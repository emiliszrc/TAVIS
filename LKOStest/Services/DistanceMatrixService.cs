using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;

namespace LKOStest.Services
{
    public class DistanceMatrixService : IDistanceMatrixService
    {
        public DistanceCalculation GetDistance(string origin, string destination)
        {
            var client = new RestClient("https://maps.googleapis.com/maps/api/distancematrix/json?");
            var request = new RestRequest(Method.GET);

            request.AddQueryParameter("origins", origin);
            request.AddQueryParameter("destinations", destination);
            request.AddQueryParameter("key", "AIzaSyBsbhG8jrghjF0VMCS90Ex7AQnaoy2HVUY");

            var response = client.Execute(request);

            var distance = JsonConvert.DeserializeObject<DistanceCalculation>(response.Content);

            return distance;
        }
    }

    public interface IDistanceMatrixService
    {
        public DistanceCalculation GetDistance(string origin, string destination);
    }


    public class Distance
    {
        public string text { get; set; }
        public int value { get; set; }
    }

    public class Duration
    {
        public string text { get; set; }
        public int value { get; set; }
    }

    public class Element
    {
        public Distance distance { get; set; }
        public Duration duration { get; set; }
        public string status { get; set; }
    }

    public class Row
    {
        public List<Element> elements { get; set; }
    }

    public class DistanceCalculation
    {
        public List<string> destination_addresses { get; set; }
        public List<string> origin_addresses { get; set; }
        public List<Row> rows { get; set; }
        public string status { get; set; }
    }
}



