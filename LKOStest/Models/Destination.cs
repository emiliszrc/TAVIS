using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LKOStest.Dtos;
using Microsoft.AspNetCore.Mvc;
using static System.String;

namespace LKOStest.Models
{
    public class Destination
    {
        public string Title { get; set; }
        
        public string Type { get; set; }

        public string LocationId { get; set; }

        public string Address { get; set; }

        public string Latitude { get; set; }

        public string Longtitude { get; set; }

        public static Destination From (Datum searchResponse)
        {
            var point = new Destination
            {
                LocationId = searchResponse.result_object.location_id,
                Title = searchResponse.result_object.name,
                Address = searchResponse.result_object.address,
                Latitude = searchResponse.result_object.latitude,
                Longtitude = searchResponse.result_object.longitude,
                Type = searchResponse.result_type
            };

            return point;
        }
    }

}
