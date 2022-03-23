namespace FullFraim.Models.Dto_s.Reviews
{
    public class OutputGiveReviewDto
    {
        public string Comment { get; set; }
        public uint Score { get; set; }
        public bool Checkbox { get; set; }
        public int PhotoId { get; set; }
        public int ContestId { get; set; }
        public int JuryId { get; set; }
    }
}
