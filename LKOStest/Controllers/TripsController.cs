using System.Collections.Generic;
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

        [HttpPost]
        [Route("{tripId}/Destinations/Reorder")]
        public IActionResult ReorderTripDestinations(string tripId, [FromBody] List<Destination> destinations)
        {
            var trip = tripService.ReorderTripDestinations(tripId, destinations);

            return Ok(trip);
        }

        [HttpPost]
        [Route("{tripId}/Reviews")]
        public IActionResult AddReviewToTrip([FromBody] ReviewRequest reviewRequest)
        {
            var review = reviewService.AddReviewToTrip(reviewRequest);

            return Ok(review);
        }

        [HttpPost]
        [Route("{tripId}/Reviews/{reviewId}/Comments")]
        public IActionResult AddCommentToReview([FromBody] CommentRequest commentRequest)
        {
            var review = reviewService.AddCommentToTrip(commentRequest);

            return Ok(review);
        }

        [HttpDelete]
        [Route("{tripId}/Reviews/{reviewId}/Comments/{commentId}")]
        public IActionResult DeleteCommentFromReview(string reviewId, string commentId)
        {
            var review = reviewService.DeleteComment(reviewId, commentId);

            return Ok(review);
        }

        [HttpGet]
        [Route("{tripId}/Reviews/{reviewId}")]
        public IActionResult GetReview(string reviewId)
        {
            var review = reviewService.GetReview(reviewId);

            return Ok(review);
        }

        [HttpGet]
        [Route("{tripId}/Reviews/")]
        public IActionResult GetTripReviews(string tripId)
        {
            var reviews = reviewService.GetReviews(tripId);

            return Ok(reviews);
        }
    }
}
