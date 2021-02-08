using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LKOStest.Controllers;
using LKOStest.Entities;
using LKOStest.Interfaces;
using LKOStest.Migrations;
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

        
        public Review GetReview(string reviewId)
        {
            var review = tripContext.Reviews
                .Include(r => r.Comments)
                .ThenInclude(c=>c.Visit)
                .Include(a => a.Approvals)
                .ThenInclude(u => u.User)
                .FirstOrDefault(r => r.Id == reviewId);

            return review ?? throw new NotFoundException();
        }

        public List<Review> GetReviewsByUserId(string userId)
        {
            var reviews = tripContext.Reviews
                .Include(r => r.Comments)
                .ThenInclude(c => c.Visit)
                .Include(r => r.Trip)
                .ThenInclude(t => t.Creator)
                .Where(r => r.Trip.Creator.Id == userId)
                .Include(a => a.Approvals)
                .ThenInclude(u => u.User)
                .Include(t => t.Reviewers)
                .ToList();

            if (!reviews.Any())
            {
                throw new NotFoundException();
            }
            
            reviews.ForEach(r => r.Comments.RemoveAll(c => c.ParentComment != null));

            return reviews;
        }

        public Review CreateReviewForTrip(string tripId)
        {
            var trip = tripContext.Trips.FirstOrDefault(t => t.Id == tripId);

            var review = new Review()
            {
                Trip = trip
            };

            tripContext.Reviews.Add(review);

            if (tripContext.SaveChanges() == 0)
            {
                throw new Exception("Failed to create review for " +
                                    $"{nameof(trip)}: {tripId}");
            }

            return GetReview(review.Id);
        }

        public Review AddCommentToTrip(CommentRequest commentRequest)
        {
            var review = tripContext.Reviews.FirstOrDefault(r => r.Id == commentRequest.ReviewId);
            var parentComment = tripContext.Comments.FirstOrDefault(c => c.Id == commentRequest.ParentCommentId);
            var visit = tripContext.Visits.FirstOrDefault(d => d.Id == commentRequest.VisitId);
            var user = tripContext.Users.FirstOrDefault(u => u.Id == commentRequest.CreatorId);

            var comment = new Comment()
            {
                Visit = visit,
                Text = commentRequest.Text,
                ParentComment = parentComment,
                Review = review,
                ElementType = commentRequest.ElementType,
                Creator = user
            };

            tripContext.Comments.Add(comment);

            if (tripContext.SaveChanges() == 0)
            {
                throw new Exception("Failed to save comment for " +
                                    $"{nameof(review)}: {commentRequest.ReviewId}," +
                                    $"{nameof(user)}: {commentRequest.CreatorId}");
            }

            return GetReview(review.Id);
        }

        public Review DeleteComment(string reviewId, string commentId)
        {
            var review = tripContext.Reviews.FirstOrDefault(r => r.Id == reviewId);
            var comment = review.Comments.FirstOrDefault(c => c.Id == commentId);

            review.Comments.Remove(comment);

            tripContext.Reviews.Update(review);

            return review;
        }

        public List<Review> GetReviewsByTripId(string tripId)
        {
            var reviews = tripContext.Reviews
                .Include(r => r.Comments)
                .ThenInclude(c => c.Visit)
                .Where(r => r.Trip.Id == tripId)
                .Include(a => a.Approvals)
                .ThenInclude(u => u.User)
                .Include(t => t.Reviewers)
                .ToList();

            if (!reviews.Any())
            {
                throw new NotFoundException();
            }

            reviews.ForEach(r => r.Comments.RemoveAll(c => c.ParentComment != null));

            return reviews;
        }

        public Review PostReviewStatus(ReviewStatusRequest request)
        {
            var review = tripContext.Reviews.FirstOrDefault(r => r.Id == request.ReviewId);
            var user = tripContext.Users.FirstOrDefault(u => u.Id == request.CreatorId);

            var approval = new Approval()
            {
                Review = review,
                User = user,
                Status = request.ReviewStatus
            };

            tripContext.Approvals.Add(approval);

            if (tripContext.SaveChanges() == 0)
            {
                throw new Exception("Failed to save review status:" +
                                    $"{nameof(review)}: {request.ReviewId}," +
                                    $"{nameof(user)}: {request.CreatorId}," +
                                    $"{nameof(request.ReviewStatus)}: {request.ReviewStatus}");
            }

            return GetReview(review.Id);
        }
    }
}
