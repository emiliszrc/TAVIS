using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LKOStest.Controllers;
using LKOStest.Dtos;
using LKOStest.Entities;

namespace LKOStest.Interfaces
{
    public interface IReviewService
    {
        Review GetReview(string reviewId);
        Review GetReviewsByTripId(string tripId);
        List<Review> GetNewReviewsByUserId(string userId);
        Review CreateReviewForTrip(ReviewRequest tripId, Models.TripValidity validity);
        Review PostReviewStatus(ReviewStatusRequest request);
        Review AddCommentToTrip(CommentRequest commentRequest);
        Review DeleteComment(string reviewId, string commentId);
        List<Review> GetAlreadyApprovedReviewsByUserId(string userId);
        List<Review> GetClosedReviewsByUserId(string userId);
        List<Review> GetNewReviewsByCreatorId(string userId);
        List<Review> GetAlreadyApprovedReviewsByCreatorId(string userId);
        List<Review> GetClosedReviewsByCreatorId(string userId);
    }
}
