using System;
using System.Collections.Generic;
using LKOStest.Dtos;
using LKOStest.Entities;
using LKOStest.Interfaces;
using LKOStest.Services;
using Microsoft.AspNetCore.Mvc;

namespace LKOStest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : Controller
    {
        private ITripService tripService;
        private IReviewService reviewService;

        public TripsController(ITripService tripService, IReviewService reviewService)
        {
            this.tripService = tripService;
            this.reviewService = reviewService;
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
        [Route("/ByClientId/{clientId}")]
        public IActionResult GetByClientId(string clientId)
        {
            var trips = tripService.GetTripsByClientId(clientId);

            return Ok(trips);
        }

        [HttpGet]
        [Route("all")]
        public IActionResult GetAllTrips()
        {
            try
            {
                var createdTrip = tripService.GetTrips();

                return Ok(createdTrip);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("{tripId}/Visits")]
        public IActionResult AddDestinationToTrip([FromBody] VisitRequest visitRequest)
        {
            var trip = tripService.AddDestinationToTrip(visitRequest);

            return Ok(trip);
        }

        [HttpDelete]
        [Route("{tripId}/Visits/{visitId}")]
        public IActionResult AddDestinationToTrip(string tripId, string visitId)
        {
            tripService.RemoveVisitFromTrip(tripId, visitId);

            return Ok();
        }

        [HttpPost]
        [Route("Locations")]
        public IActionResult AddLocation(string tripId, [FromBody] LocationRequest locationRequest)
        {
            var trip = tripService.AddLocation(tripId, locationRequest);

            return Ok(trip);
        }

        [HttpPost]
        [Route("{tripId}/Destinations/Reorder")]
        public IActionResult ReorderTripDestinations(string tripId, [FromBody] List<Visit> destinations)
        {
            var trip = tripService.ReorderTripDestinations(tripId, destinations);

            return Ok(trip);
        }
    }
}
