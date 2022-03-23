using FullFraim.Data.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FullFraim.Data.Models
{
    public class Photo : DeletableEntity<int>
    {
        [Required]
        [StringLength(maximumLength: 20)]
        public string Title { get; set; }

        [StringLength(maximumLength: 2000)]
        public string Story { get; set; }

        [Required]
        public string Url { get; set; }

        public int ContestId { get; set; }
        public Contest Contest { get; set; }

        public ParticipantContest Participant { get; set; }

        public ICollection<PhotoReview> PhotoReviews { get; set; }
    }
}
