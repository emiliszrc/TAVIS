using System;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using System.Net.Http;
using System.Threading;

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
            request.AddQueryParameter("key", "AIzaSyBTy3bxl0b3SAmoeef4cb08uQnnJM3U9nM");

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var distance = JsonConvert.DeserializeObject<DistanceCalculation>(response.Content);
                return distance;
            }

            Console.WriteLine($"Distance call failed with " +
                              $"{nameof(response.ErrorMessage)}: {response.ErrorMessage}, " +
                              $"{nameof(response.StatusCode)}: {response.StatusCode.ToString()}");
            throw new HttpRequestException(response.StatusCode.ToString());
        }
    }
}



