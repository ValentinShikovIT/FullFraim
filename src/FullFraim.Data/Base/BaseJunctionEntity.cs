using System;
using System.ComponentModel.DataAnnotations;

namespace FullFraim.Data.Base
{
    public class BaseJunctionEntity : IModifiable
    {
        [Required]
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
