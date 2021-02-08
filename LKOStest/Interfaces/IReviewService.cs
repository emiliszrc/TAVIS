using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LKOStest.Controllers;
using LKOStest.Entities;

namespace LKOStest.Interfaces
{
    public interface IReviewService
    {
        Review GetReview(string reviewId);
        List<Review> GetReviewsByTripId(string tripId);
        List<Review> GetReviewsByUserId(string userId);
        Review CreateReviewForTrip(string tripId);
        Review PostReviewStatus(ReviewStatusRequest request);
        Review AddCommentToTrip(CommentRequest commentRequest);
        Review DeleteComment(string reviewId, string commentId);
    }
}
