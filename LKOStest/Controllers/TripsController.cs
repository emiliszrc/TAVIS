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
        [Route("ByClientId/{clientId}")]
        public IActionResult GetByClientId(string clientId)
        {
            var trips = tripService.GetTripsByClientId(clientId);

            return Ok(trips);
        }

        [HttpGet]
        [Route("createdByUser/{userId}")]
        public IActionResult GetUserTrips(string userId)
        {
            try
            {
                var createdTrips = tripService.GetUserTrips(userId);

                return Ok(createdTrips);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("createdByOrganisation/{userId}")]
        public IActionResult GetOrganisationTrips(string userId)
        {
            try
            {
                var createdTrips = tripService.GetOrganisationTrips(userId);

                return Ok(createdTrips);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
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

        [HttpGet]
        [Route("final/{userId}")]
        public IActionResult GetAllFinalTrips(string userId)
        {
            try
            {
                var finalTrips = tripService.GetFinalTrips(userId);

                return Ok(finalTrips);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete]
        [Route("{tripId}")]
        public IActionResult DeleteTrip(string tripId)
        {
            try
            {
                tripService.DeleteTrip(tripId);

                return Ok();
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

        [HttpGet]
        [Route("Visits/{visitId}")]
        public IActionResult GetVisitById(string visitId)
        {
            var visit = tripService.GetVisit(visitId);

            return Ok(visit);
        }

        [HttpPost]
        [Route("Visits/{visitId}")]
        public IActionResult UpdateVisit(string visitId, [FromBody] VisitRequest visitRequest)
       {
            var visit = tripService.UpdateVisit(visitId, visitRequest);

            return Ok(visit);
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

        [HttpPost]
        [Route("{tripId}/Reuse")]
        public IActionResult Reuse(string tripId, [FromBody] ReuseRequest reuseRequest)
        {
            var trip = tripService.ReuseTrip(tripId, reuseRequest);

            return Ok(trip);
        }

        [HttpGet]
        [Route("Validate/{tripId}")]
        public IActionResult ValidateTrip(string tripId)
        {
            var trip = tripService.GetTrip(tripId);

            var validity = tripService.ValidateTrip(trip);

            return Ok(validity);
        }

        [HttpGet]
        [Route("{tripId}/Comments")]
        public IActionResult GetTripComments(string tripId)
        {
            var comments = tripService.GetComments(tripId);

            return Ok(comments);
        }

        [HttpGet]
        [Route("{tripId}/Review")]
        public IActionResult GetTripReview(string tripId)
        {
            var comments = tripService.GetReview(tripId);

            return Ok(comments);
        }

        [HttpPost]
        [Route("Restore")]
        public IActionResult RunRestore()
        {
            tripService.RestoreStatuses();

            return Ok();
        }
    }

    public class ReuseRequest
    {
        public string ReusingTripId { get; set; } = string.Empty;
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public string Title { get; set; } = string.Empty;
        public string CreatorId { get; set; } = string.Empty;
    }
}
