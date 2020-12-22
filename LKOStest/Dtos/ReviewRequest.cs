namespace LKOStest.Controllers
{
    public class ReviewRequest
    {
        public string TripId { get; set; }
        public string UserId { get; set; }
        public string Text { get; set; }
        public string ApprovalStatus { get; set; }
    }
}