using FullFraim.Models.Dto_s.Reviews;

namespace FullFraim.Models.ViewModels.Contest
{
    public class GiveReviewViewModel
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int PhotoId { get; set; }
        public string PhotoUrl { get; set; }
        public int JuryId { get; set; }
        public int ReviewId { get; set; }
        public int ContestId { get; set; }
        public InputGiveReviewDto Review { get; set; }
        public bool HasJuryGivenReview { get; set; }
    }
}
