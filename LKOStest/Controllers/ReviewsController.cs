using System;
using LKOStest.Dtos;
using LKOStest.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LKOStest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : Controller
    {
        private IReviewService reviewService;
        private ITripService tripService;

        public ReviewsController(IReviewService reviewService, ITripService tripService)
        {
            this.reviewService = reviewService;
            this.tripService = tripService;
        }

        [HttpPost]
        public IActionResult CreateReview([FromBody] ReviewRequest request)
        {
            try
            {
                var trip = tripService.GetTrip(request.TripId);
                var validity = tripService.ValidateTrip(trip);

                if (validity.IsValid && request.IgnoreWarnings)
                {
                    var review = reviewService.CreateReviewForTrip(request, validity);
                    return Ok(review);
                }

                return BadRequest();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("{reviewId}/Comments")]
        public IActionResult AddCommentToReview([FromBody] CommentRequest commentRequest)
        {
            try
            {
                var review = reviewService.AddCommentToTrip(commentRequest);

                return Ok(review);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, e);
            }
        }

        [HttpDelete]
        [Route("{reviewId}/Comments/{commentId}")]
        public IActionResult DeleteCommentFromReview(string reviewId, string commentId)
        {
            try
            {
                var review = reviewService.DeleteComment(reviewId, commentId);

                return Ok(review);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e);
            }
        }

        [HttpGet]
        [Route("{reviewId}")]
        public IActionResult GetReview(string reviewId)
        {
            try
            {
                var review = reviewService.GetReview(reviewId);

                return Ok(review);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e);
            }
        }

        [HttpGet]
        [Route("by-trip/")]
        public IActionResult GetReviewsByTripId([FromQuery] string tripId)
        {
            try
            {
                var reviews = reviewService.GetReviewsByTripId(tripId);

                return Ok(reviews);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e);
            }
        }

        [HttpGet]
        [Route("by-user/forApproval")]
        public IActionResult GetReviewsByUserId([FromQuery] string userId)
        {
            try
            {
                var reviews = reviewService.GetNewReviewsByUserId(userId);

                return Ok(reviews);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e);
            }
        }

        [HttpGet]
        [Route("by-user/alreadyApproved")]
        public IActionResult GetApprovedReviewsByUserId([FromQuery] string userId)
        {
            try
            {
                var reviews = reviewService.GetAlreadyApprovedReviewsByUserId(userId);

                return Ok(reviews);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e);
            }
        }

        [HttpGet]
        [Route("by-user/closed")]
        public IActionResult GetClosedReviewsByUserId([FromQuery] string userId)
        {
            try
            {
                var reviews = reviewService.GetClosedReviewsByUserId(userId);

                return Ok(reviews);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e);
            }
        }

        [HttpGet]
        [Route("by-creator/forApproval")]
        public IActionResult GetReviewsByCreatorId([FromQuery] string userId)
        {
            try
            {
                var reviews = reviewService.GetNewReviewsByCreatorId(userId);

                return Ok(reviews);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e);
            }
        }

        [HttpGet]
        [Route("by-creator/alreadyApproved")]
        public IActionResult GetApprovedReviewsByCreatorId([FromQuery] string userId)
        {
            try
            {
                var reviews = reviewService.GetAlreadyApprovedReviewsByCreatorId(userId);

                return Ok(reviews);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e);
            }
        }

        [HttpGet]
        [Route("by-creator/closed")]
        public IActionResult GetClosedReviewsByCreatorId([FromQuery] string userId)
        {
            try
            {
                var reviews = reviewService.GetClosedReviewsByCreatorId(userId);

                return Ok(reviews);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e);
            }
        }

        [HttpPost]
        [Route("{reviewId}/Status")]
        public IActionResult PostStatus([FromBody] ReviewStatusRequest request)
        {
            try
            {
                var review = reviewService.PostReviewStatus(request);

                return Ok(review);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e);
            }
        }
    }
}