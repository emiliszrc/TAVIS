using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LKOStest.Controllers;
using LKOStest.Entities;
using LKOStest.Interfaces;
using Microsoft.EntityFrameworkCore;

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

            return GetReview(review.Id);
        }

        public Review AddCommentToTrip(CommentRequest commentRequest)
        {
            var review = tripContext.Reviews.FirstOrDefault(r => r.Id == commentRequest.ReviewId);
            var parentComment = tripContext.Comments.FirstOrDefault(c => c.Id == commentRequest.ParentCommentId);
            var visit = tripContext.Visits.FirstOrDefault(d => d.Id == commentRequest.VisitId);

            var comment = new Comment()
            {
                Visit = visit,
                Text = commentRequest.Text,
                ParentComment = parentComment,
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
            return tripContext.Reviews
                .Include(r => r.Comments)
                .ThenInclude(c=>c.Visit)
                .FirstOrDefault(r => r.Id == reviewId);
        }

        public Review DeleteComment(string reviewId, string commentId)
        {
            var review = tripContext.Reviews.FirstOrDefault(r => r.Id == reviewId);
            var comment = review.Comments.FirstOrDefault(c => c.Id == commentId);

            review.Comments.Remove(comment);

            tripContext.Reviews.Update(review);

            return review;
        }

        public List<Review> GetReviews(string tripId, string userId = null)
        {
            var reviews =  userId != null 
                ? tripContext.Reviews
                    .Include(r=>r.Comments)
                    .ThenInclude(c => c.Visit)
                    .Include(r => r.User)
                    .Where(r => r.Trip.Id == tripId && r.User.Id == userId)
                    .ToList() 
                : tripContext.Reviews
                    .Include(r=>r.Comments)
                    .ThenInclude(c => c.Visit)
                    .Include(r => r.User)
                    .Where(r => r.Trip.Id == tripId)
                    .ToList();

            reviews.ForEach(r => r.Comments.RemoveAll(c => c.ParentComment != null));

            return reviews;
        }
    }
}
