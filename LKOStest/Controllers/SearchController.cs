using System.Linq;
using LKOStest.Interfaces;
using LKOStest.Models;
using LKOStest.Services;
using Microsoft.AspNetCore.Mvc;

namespace LKOStest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : Controller
    {
        private ISearchService searchService;
        private IDistanceMatrixService distanceMatrixService;

        public SearchController(ISearchService searchService, IDistanceMatrixService distanceMatrixService)
        {
            this.searchService = searchService;
            this.distanceMatrixService = distanceMatrixService;
        }

        [HttpGet]
        [Route("location")]
        public IActionResult Get([FromQuery] string location)
        {
            if (string.IsNullOrEmpty(location))
            {
                return BadRequest($"\"location\" was null or empty");
            }

            var response = searchService.SearchForLocation(location);
            var restaurant = response.data.FirstOrDefault(r => r.result_type == "restaurants");
            var details = searchService.GetRestaurantDetails(restaurant.result_object.location_id);
            var attraction = response.data.FirstOrDefault(r => r.result_type == "things_to_do");
            var attDetails = searchService.GetAttractionDetails(attraction.result_object.location_id);

            return Ok(response.data.Select(Destination.From));
        }

        [HttpGet]
        [Route("distance")]
        public IActionResult GetDistance([FromQuery] string origin, [FromQuery] string destination)
        {
            if (string.IsNullOrEmpty(origin))
            {
                return BadRequest($"\"origin\" was null or empty");
            }
            if (string.IsNullOrEmpty(destination))
            {
                return BadRequest($"\"destination\" was null or empty");
            }
            return Ok(distanceMatrixService.GetDistance(origin, destination));
        }

        [HttpGet]
        [Route("holidays")]
        public IActionResult GetHolidays([FromQuery] string year, [FromQuery] string countryCode)
        {
            var holidays = searchService.SearchForHolidays(countryCode, year);

            return Ok(holidays);
        }

        [HttpGet]
        [Route("reverseGeocoding")]
        public IActionResult GetReverseGeocoding([FromQuery] string latitude, [FromQuery] string longtitude)
        {
            var countryCode = searchService.GetCountryCode(latitude, longtitude);

            return Ok(countryCode);
        }
    }
}