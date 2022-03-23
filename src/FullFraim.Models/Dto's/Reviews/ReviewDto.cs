namespace FullFraim.Models.Dto_s.Reviews
{
    public class ReviewDto
    {
        public int ReviewId { get; set; }
        public string AuthorName { get; set; }
        public string Comment { get; set; }
        public uint Score { get; set; }
        public int PhotoId { get; set; }
        public int AuthorId { get; set; }
        public bool IsDisqualified { get; set; }
    }
}
