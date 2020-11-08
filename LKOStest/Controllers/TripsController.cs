using LKOStest.Dtos;
using LKOStest.Entities;
using LKOStest.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LKOStest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : Controller
    {
        private ITripService tripService;

        public TripsController(ITripService tripService)
        {
            this.tripService = tripService;
        }

        [HttpPost]
        public IActionResult Post([FromBody] TripRequest tripRequest)
        {
            var createdTrip = tripService.CreateNewTrip(tripRequest);

            return Ok(createdTrip);
        }

        [HttpGet]
        [Route("{tripId}")]
        public IActionResult Get(string tripId)
        {
            var createdTrip = tripService.GetTrip(tripId);

            return Ok(createdTrip);
        }

        [HttpGet]
        [Route("all")]
        public IActionResult GetAllTrips()
        {
            var createdTrip = tripService.GetTrips();

            return Ok(createdTrip);
        }

        [HttpPost]
        [Route("{tripId}/Destinations")]
        public IActionResult AddDestinationToTrip(string tripId, [FromBody] Destination destination)
        {
            var trip = tripService.AddDestinationToTrip(tripId, destination);

            return Ok(trip);
        }
    }
}
