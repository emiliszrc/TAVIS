﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LKOStest.Controllers;
using LKOStest.Entities;

namespace LKOStest.Interfaces
{
    public interface IReviewService
    {
        Review AddReviewToTrip(ReviewRequest reviewRequest);

        Review AddCommentToTrip(CommentRequest commentRequest);

        Review GetReview(string reviewId);

        Review DeleteComment(string reviewId, string commentId);
        List<Review> GetReviews(string tripId, string userId = null);
        Review PostReviewStatus(ReviewStatusRequest request);
    }
}
