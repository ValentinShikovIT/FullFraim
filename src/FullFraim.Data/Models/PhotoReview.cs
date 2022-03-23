using FullFraim.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace FullFraim.Data.Models
{
    public class PhotoReview : DeletableEntity<int>
    {
        [Required]
        public string Comment { get; set; }

        [Range(0, 10)]
        public uint Score { get; set; }

        public bool Checkbox { get; set; }

        public int PhotoId { get; set; }
        public Photo Photo { get; set; }

        public int JuryContestId { get; set; }
        public JuryContest JuryContest { get; set; }
    }
}
