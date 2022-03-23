using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FullFraim.Models.ViewModels.Enrolling
{
    public class EnrollViewModel
    {
        public int UserId { get; set; }

        public int ContestId { get; set; }

        [Required]
        public IFormFile Photo { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        [Display(Name = "Image Description")]
        public string ImageDescription { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3)]
        [Display(Name = "Image Title")]
        public string ImageTitle { get; set; }
    }
}
