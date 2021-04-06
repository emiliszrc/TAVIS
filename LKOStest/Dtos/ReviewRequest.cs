using System.Collections.Generic;

namespace LKOStest.Dtos
{
    public class ReviewRequest
    {
        public string TripId { get; set; }
        public List<string> Reviewers { get; set; }
        public bool IgnoreWarnings { get; set; }
    }
}