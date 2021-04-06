using LKOStest.Entities;

namespace LKOStest.Controllers
{
    public class ReviewStatusRequest
    {
        public string ReviewId { get; set; }
        public string CreatorId { get; set; }
        public ApprovalStatus ReviewStatus { get; set; }
    }
}