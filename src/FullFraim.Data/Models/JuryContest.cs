using FullFraim.Data.Base;
using System.Collections.Generic;

namespace FullFraim.Data.Models
{
    public class JuryContest : DeletableEntity<int>
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int ContestId { get; set; }
        public Contest Contest { get; set; }

        public ICollection<PhotoReview> PhotoReviews { get; set; }
    }
}
