using System.ComponentModel.DataAnnotations;

namespace FullFraim.Models.Dto_s.AccountAPI
{
    public class RegisterInputModel_API
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
