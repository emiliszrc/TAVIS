using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LKOStest.Controllers;
using LKOStest.Dtos;
using LKOStest.Entities;
using LKOStest.Interfaces;
using LKOStest.Migrations;
using LKOStest.Models;
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
                .ThenInclude(c => c.Visit)
                .Include(a => a.Approvals)
                .ThenInclude(u => u.User)
                .Include(r => r.Reviewers)
                .ThenInclude(r => r.User)
                .Include(r => r.Trip)
                .Include(r=>r.Warnings)
                .ThenInclude(w => w.Visit)
                .FirstOrDefault(r => r.Id == reviewId);

            review?.Comments.RemoveAll(c => c.ParentComment != null);

            return review ?? throw new NotFoundException();
        }

        public List<Review> GetNewReviewsByUserId(string userId)
        {
            var reviews = tripContext.Reviews
                .Include(r => r.Comments)
                .ThenInclude(c => c.Visit)
                .Include(r=>r.Comments)
                .ThenInclude(c=>c.Creator)
                .Include(r => r.Trip)
                .ThenInclude(t => t.Creator)
                .Include(a => a.Approvals)
                .ThenInclude(u => u.User)
                .Include(t => t.Reviewers)
                .ThenInclude(r => r.User)
                .Include(r => r.Warnings)
                .ThenInclude(w => w.Visit)
                .Where(r => r.Reviewers.Any(rr=>rr.User.Id == userId) && r.Status == ReviewStatus.New)
                .ToList();

            reviews = reviews.Where(r => r.Approvals.All(a => a.User.Id != userId)).ToList();

            reviews.ForEach(r=>r.Comments.RemoveAll(c=>c.ParentComment != null));

            return reviews;
        }

        public List<Review> GetAlreadyApprovedReviewsByUserId(string userId)
        {
            var reviews = tripContext.Reviews
                .Include(r => r.Comments)
                .ThenInclude(c => c.Visit)
                .Include(r => r.Comments)
                .ThenInclude(c => c.Creator)
                .Include(r => r.Trip)
                .ThenInclude(t => t.Creator)
                .Include(a => a.Approvals)
                .ThenInclude(u => u.User)
                .Include(t => t.Reviewers)
                .ThenInclude(r => r.User)
                .Include(r => r.Warnings)
                .ThenInclude(w => w.Visit)
                .Where(r => r.Reviewers.Any(rr => rr.User.Id == userId) && r.Status == ReviewStatus.New)
                .ToList();

            reviews = reviews.Where(r => r.Approvals.Any(a => a.User.Id == userId)).ToList();

            reviews.ForEach(r => r.Comments.RemoveAll(c => c.ParentComment != null));

            return reviews;
        }

        public List<Review> GetClosedReviewsByUserId(string userId)
        {
            var reviews = tripContext.Reviews
                .Include(r => r.Comments)
                .ThenInclude(c => c.Visit)
                .Include(r => r.Comments)
                .ThenInclude(c => c.Creator)
                .Include(r => r.Trip)
                .ThenInclude(t => t.Creator)
                .Include(a => a.Approvals)
                .ThenInclude(u => u.User)
                .Include(t => t.Reviewers)
                .ThenInclude(r => r.User)
                .Include(r => r.Warnings)
                .ThenInclude(w => w.Visit)
                .Where(r => r.Reviewers.Any(rr => rr.User.Id == userId) && r.Status != ReviewStatus.New)
                .ToList();

            reviews = reviews.Where(r => r.Approvals.Any(a => a.User.Id == userId)).ToList();

            reviews.ForEach(r => r.Comments.RemoveAll(c => c.ParentComment != null));

            return reviews;
        }

        public List<Review> GetNewReviewsByCreatorId(string userId)
        {
            var reviews = tripContext.Reviews
                .Include(r => r.Comments)
                .ThenInclude(c => c.Visit)
                .Include(r => r.Comments)
                .ThenInclude(c => c.Creator)
                .Include(r => r.Trip)
                .ThenInclude(t => t.Creator)
                .Include(a => a.Approvals)
                .ThenInclude(u => u.User)
                .Include(t => t.Reviewers)
                .ThenInclude(r => r.User)
                .Include(r => r.Warnings)
                .ThenInclude(w => w.Visit)
                .Where(r => r.Trip.Creator.Id == userId && r.Status == ReviewStatus.New)
                .ToList();

            reviews = reviews.Where(r => r.Approvals.All(a => a.User.Id != userId)).ToList();

            reviews.ForEach(r => r.Comments.RemoveAll(c => c.ParentComment != null));

            return reviews;
        }

        public List<Review> GetAlreadyApprovedReviewsByCreatorId(string userId)
        {
            var reviews = tripContext.Reviews
                .Include(r => r.Comments)
                .ThenInclude(c => c.Visit)
                .Include(r => r.Comments)
                .ThenInclude(c => c.Creator)
                .Include(r => r.Trip)
                .ThenInclude(t => t.Creator)
                .Include(a => a.Approvals)
                .ThenInclude(u => u.User)
                .Include(t => t.Reviewers)
                .ThenInclude(r => r.User)
                .Include(r => r.Warnings)
                .ThenInclude(w => w.Visit)
                .Where(r => r.Trip.Creator.Id == userId && r.Status == ReviewStatus.New)
                .ToList();

            reviews = reviews.Where(r => r.Approvals.Any(a => a.User.Id == userId)).ToList();

            reviews.ForEach(r => r.Comments.RemoveAll(c => c.ParentComment != null));

            return reviews;
        }

        public List<Review> GetClosedReviewsByCreatorId(string userId)
        {
            var reviews = tripContext.Reviews
                .Include(r => r.Comments)
                .ThenInclude(c => c.Visit)
                .Include(r => r.Comments)
                .ThenInclude(c => c.Creator)
                .Include(r => r.Trip)
                .ThenInclude(t => t.Creator)
                .Include(a => a.Approvals)
                .ThenInclude(u => u.User)
                .Include(t => t.Reviewers)
                .ThenInclude(r => r.User)
                .Include(r => r.Warnings)
                .ThenInclude(w=>w.Visit)
                .Where(r => r.Trip.Creator.Id == userId && r.Status != ReviewStatus.New)
                .ToList();

            reviews = reviews.Where(r => r.Approvals.Any(a => a.User.Id == userId)).ToList();

            reviews.ForEach(r => r.Comments.RemoveAll(c => c.ParentComment != null));

            return reviews;
        }

        public Review CreateReviewForTrip(ReviewRequest request, TripValidity validity)
        {
            var currentReviewForTrip =
                tripContext.Reviews.FirstOrDefault(
                    t => t.Trip.Id == request.TripId && t.Status == ReviewStatus.Approved);

            var warnings = validity.Reasons.Select(r => new Warning
            {
                WarningCode = r.Code,
                WarningText = r.Text,
                IsBlocker = r.IsBlocker,
                Visit = tripContext.Visits.FirstOrDefault(v=>v.Id == r.VisitId)
            }).ToList();

            if (currentReviewForTrip != null)
            {
                throw new ObjectAlreadyExists("Failed to create review as ongoing review for this trip already exists");
            }

            var trip = tripContext.Trips.FirstOrDefault(t => t.Id == request.TripId);

            trip.TripStatus = TripStatus.InReview;

            tripContext.Trips.Update(trip);

            var review = new Review()
            {
                Trip = trip,
                Status = ReviewStatus.New,
                Warnings = warnings
            };

            tripContext.Reviews.Add(review);

            if (tripContext.SaveChanges() == 0)
            {
                throw new Exception("Failed to create review for " +
                                    $"{nameof(trip)}: {request.TripId}");
            }

            var reviewers = request.Reviewers
                .Select(reviewer => new Reviewer
                {
                    User = tripContext.Users.FirstOrDefault(u => u.Id == reviewer),
                    Review = tripContext.Reviews.FirstOrDefault(r => r.Id == review.Id)
                })
                .ToList();

            tripContext.Reviewers.AddRange(reviewers);

            if (tripContext.SaveChanges() == 0)
            {
                throw new Exception("Failed to create reviewers for " +
                                    $"{nameof(review)}: {review.Id}");
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

            RevalidateReviewStatus(review, request.ReviewStatus);

            return GetReview(review.Id);
        }

        private void RevalidateReviewStatus(Review review, ApprovalStatus requestReviewStatus)
        {
            switch (requestReviewStatus)
            {
                case ApprovalStatus.Rejecting:
                    UpdateReviewToStatus(review.Id, ReviewStatus.Rejected);
                    break;
                case ApprovalStatus.Cancelled:
                    UpdateReviewToStatus(review.Id, ReviewStatus.Cancelled);
                    break;
                case ApprovalStatus.Approved:
                {
                    var approvals = tripContext.Approvals.Where(a => a.Review.Id == review.Id).ToList();

                    var approvingApprovals = approvals
                        .Where(approval => approval.Status.Equals(ApprovalStatus.Approved))
                        .ToList();

                    var trip = tripContext.Reviews
                        .Include(r => r.Trip)
                        .ThenInclude(t => t.Organisation)
                        .FirstOrDefault(r => review.Id == r.Id).Trip;

                    if (approvingApprovals.Count >= trip.Organisation.RequiredReviewerCount)
                    {
                        UpdateReviewToStatus(review.Id, ReviewStatus.Approved);
                    }

                    break;
                }
            }
        }

        private void UpdateReviewToStatus(string reviewId, ReviewStatus approved)
        {
            var review = GetReview(reviewId);

            switch (review.Status)
            {
                case ReviewStatus.Cancelled:
                case ReviewStatus.Approved:
                case ReviewStatus.Rejected:
                    break;
                case ReviewStatus.New:
                    review.Status = approved;
                    var trip = review.Trip;
                    trip.TripStatus = TripStatus.Final;
                    tripContext.Reviews.Update(review);
                    tripContext.Trips.Update(trip);
                    tripContext.SaveChanges();
                    break;
            }
        }
    }

    public enum ReviewStatus
    {
        New,
        NeedsWork,
        Rejected,
        Cancelled,
        Approved
    }
}
