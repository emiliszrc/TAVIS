using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LKOStest.Controllers;
using LKOStest.Entities;
using LKOStest.Interfaces;

namespace LKOStest.Services
{
    public class ReviewService : IReviewService
    {
        private TripContext tripContext;

        public ReviewService(TripContext tripContext)
        {
            this.tripContext = tripContext;
        }

        public Review AddReviewToTrip(ReviewRequest reviewRequest)
        {
            var trip = tripContext.Trips.FirstOrDefault(t => t.Id == reviewRequest.TripId);
            var user = tripContext.Users.FirstOrDefault(u => u.Id == reviewRequest.UserId);

            var review = new Review()
            {
                ApprovalStatus = reviewRequest.ApprovalStatus,
                Trip = trip,
                User = user
            };

            tripContext.Reviews.Add(review);

            if (tripContext.SaveChanges() == 0)
            {
                throw new Exception("Failed to save review data");
            }

            return new Review();
        }

        public Review AddCommentToTrip(CommentRequest commentRequest)
        {
            var review = tripContext.Reviews.FirstOrDefault(r => r.Id == commentRequest.ReviewId);

            var comment = new Comment()
            {
                DestinationIndex = commentRequest.DestinationIndex,
                Text = commentRequest.Text,
                Review = review
            };

            tripContext.Comments.Add(comment);

            if (tripContext.SaveChanges() == 0)
            {
                throw new Exception("Failed to save comment data");
            }

            return GetReview(review.Id);
        }

        public Review GetReview(string reviewId)
        {
            return tripContext.Reviews.FirstOrDefault(r => r.Id == reviewId);
        }
    }
}
