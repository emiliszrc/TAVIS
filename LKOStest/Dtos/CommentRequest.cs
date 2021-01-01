namespace LKOStest.Controllers
{
    public class CommentRequest
    {
        public string ReviewId { get; set; }
        public string VisitId { get; set; }
        public string ParentCommentId { get; set; }
        public string Text { get; set; }
    }
}