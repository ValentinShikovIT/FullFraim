using FullFraim.Models.ViewModels.Contest;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FullFraim.Models.Contest.ViewModels
{
    public class CreateContestViewModel
    {
        [Required(ErrorMessage = "*Required")]
        [StringLength(20, MinimumLength = 5)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Cover")]
        public IFormFile Cover { get; set; }

        [Display(Name = "Cover Url")]
        public string Cover_Url { get; set; }

        [Required(ErrorMessage = "*Required")]
        [StringLength(maximumLength: 250)]
        [Display(Name = "Decription")]
        public string Description { get; set; }

        public PhasesHelperModel Phases { get; set; }

        [Required(ErrorMessage = "*Required")]
        [Display(Name = "Contest Category")]
        public int ContestCategoryId { get; set; }

        [Required(ErrorMessage = "*Required")]
        [Display(Name = "Contest Type")]
        public int ContestTypeId { get; set; }

        public ICollection<int> Juries { get; set; } = new HashSet<int>();
        public ICollection<int> Participants { get; set; } = new HashSet<int>();
    }
}
