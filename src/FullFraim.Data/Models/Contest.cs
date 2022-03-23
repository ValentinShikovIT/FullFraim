using FullFraim.Data.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FullFraim.Data.Models
{
    public class Contest : DeletableEntity<int>
    {
        [Required]
        [StringLength(maximumLength: 20)]
        public string Name { get; set; }

        public string Cover_Url { get; set; }

        [Required]
        public string Description { get; set; }

        public int ContestCategoryId { get; set; }
        public ContestCategory ContestCategory { get; set; }

        public int ContestTypeId { get; set; }
        public ContestType ContestType { get; set; }

        public ICollection<ContestPhase> ContestPhases { get; set; }

        public ICollection<ParticipantContest> ParticipantContests { get; set; }

        public ICollection<JuryContest> JuryContests { get; set; }

        public ICollection<Photo> Photos { get; set; }
    }
}
