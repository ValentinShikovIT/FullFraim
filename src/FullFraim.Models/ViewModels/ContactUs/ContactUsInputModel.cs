using System.ComponentModel.DataAnnotations;

namespace FullFraim.Models.ViewModels.ContactUs
{
    public class ContactUsInputModel
    {
        [Required]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
