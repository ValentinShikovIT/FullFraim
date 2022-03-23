using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace FullFraim.Models.Dto_s.PhotoJunkies
{
    public class InputEnrollForContestModel
    {
        [Range(0, int.MaxValue)]
        public int UserId { get; set; }

        [Range(0, int.MaxValue)]
        public int ContestId { get; set; }

        [Required]
        public IFormFile Photo { get; set; }

        [Required]
        [StringLength(maximumLength: 2000, MinimumLength = 5)]
        public string ImageDescription { get; set; }

        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 2)]
        public string ImageTitle { get; set; }
    }
}
