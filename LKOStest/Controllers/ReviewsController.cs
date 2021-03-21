using System;
using LKOStest.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LKOStest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : Controller
    {
        private IReviewService reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

        [HttpPost]
        public IActionResult CreateReview([FromBody] ReviewRequest request)
        {
            try
            {
                var review = reviewService.CreateReviewForTrip(request);

                return Ok(review);
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
        [Route("by-user/")]
        public IActionResult GetReviewsByUserId([FromQuery] string userId)
        {
            try
            {
                var reviews = reviewService.GetReviewsByUserId(userId);

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