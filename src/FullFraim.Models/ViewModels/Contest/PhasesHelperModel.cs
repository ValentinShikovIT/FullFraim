using FullFraim.Models.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace FullFraim.Models.ViewModels.Contest
{
    public class PhasesHelperModel
    {
        public DateTime StartDate_PhaseI { get; set; }

        [Display(Name = "End Date of Phase I")]
        [ContestDatesAttr]
        public DateTime EndDate_PhaseI { get; set; }

        public DateTime StartDate_PhaseII { get; set; }

        [Required]
        [Display(Name = "Phase Two Duration")]
        [PhaseIIDurationAttr]
        public int PhaseII_Duration { get; set; }
        public DateTime EndDate_PhaseII
        {
            get
            {
                return EndDate_PhaseI
                    .AddHours(PhaseII_Duration);
            }
        }

        public DateTime StartDate_PhaseIII { get; set; }
        public DateTime EndDate_PhaseIII { get; set; }
    }
}
