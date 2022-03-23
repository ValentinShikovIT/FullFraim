using FullFraim.Models.Dto_s.Phases;
using FullFraim.Models.Dto_s.Reviews;
using System.Collections.Generic;

namespace FullFraim.Models.ViewModels.Contest
{
    public class ContestSubmissionViewModel
    {
        public int PhotoId { get; set; }
        public string ContestName { get; set; }
        public string ContestCategory { get; set; }
        public int AuthorId { get; set; }
        public string Image_Url { get; set; }
        public string Description { get; set; }
        public string AuthorName { get; set; }
        public double Score { get; set; }
        public bool IsCurrentUserSubmission { get; set; }
        public PhaseDto ActivePhase { get; set; }
        public int ContestId { get; set; }
        public bool HasJuryGivenSubmission { get; set; }
        public GiveReviewViewModel Review { get; set; } = new GiveReviewViewModel();
        public IEnumerable<ReviewDto> Reviews { get; set; }
    }
}
