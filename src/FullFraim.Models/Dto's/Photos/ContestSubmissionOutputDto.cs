using FullFraim.Models.Dto_s.Phases;
using FullFraim.Models.Dto_s.Reviews;
using System.Collections.Generic;

namespace FullFraim.Models.Dto_s.Photos
{
    public class ContestSubmissionOutputDto
    {
        public int contestId;
        public string ContestName { get; set; }
        public string ContestCategory { get; set; }
        public int PhotoId { get; set; }
        public string AuthorName { get; set; }
        public int AuthorId { get; set; }
        public string PhotoTitle { get; set; }
        public string PhotoUrl { get; set; }
        public string Description { get; set; }
        public double Score { get; set; }
        public PhaseDto ActivePhase
        {
            get
            {
                foreach (var phase in PhasesInfo)
                {
                    if (phase.IsActive)
                    {
                        return phase;
                    }
                }

                return null;
            }
        }
        public ReviewDto Review { get; set; }
        public bool HasJuryGivenReview { get; set; }
        public ICollection<ReviewDto> Reviews { get; set; }
        public ICollection<PhaseDto> PhasesInfo { get; set; }
    }
}
