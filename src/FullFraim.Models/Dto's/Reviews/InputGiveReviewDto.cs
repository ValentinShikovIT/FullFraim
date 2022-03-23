using System.ComponentModel.DataAnnotations;

namespace FullFraim.Models.Dto_s.Reviews
{
    public class InputGiveReviewDto
    {
        [Required]
        [MaxLength(2000)]
        public string Comment { get; set; }

        [Required]
        [Range(1, 10)]
        public uint Score { get; set; }
        public bool Checkbox { get; set; }

        [Required]
        public int PhotoId { get; set; }

        [Required]
        public int JuryId { get; set; }

        [Required]
        public int ContestId { get; set; }
    }
}
