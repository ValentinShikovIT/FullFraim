using System.ComponentModel.DataAnnotations;

namespace FullFraim.Models.Dto_s.AccountAPI
{
    public class InputLoginModel_API
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
