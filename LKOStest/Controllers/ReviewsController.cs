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
        [Route("{reviewId}/Comments")]
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

        [HttpPost]
        [Route("{reviewId}")]
        public IActionResult PostStatus([FromBody] ReviewStatusRequest request)
        {
            var review = reviewService.PostReviewStatus(request);

            return Ok(review);
        }
    }

    public class ReviewStatusRequest
    {
        public string CreatorId { get; set; }
        public string ReviewStatus { get; set; }
    }
}