using System.Collections.Generic;
using LKOStest.Entities;

namespace LKOStest.Controllers
{
    public class ReviewRequest
    {
        public string TripId;
        public List<string> Reviewers;
    }
}