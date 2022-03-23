using FullFraim.Data.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FullFraim.Data.Models
{
    public class Phase : DeletableEntity<int>
    {
        [Required]
        [StringLength(maximumLength: 20)]
        public string Name { get; set; }

        public ICollection<ContestPhase> Contests { get; set; }
    }
}
