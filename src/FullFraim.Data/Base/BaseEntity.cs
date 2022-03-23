using System;
using System.ComponentModel.DataAnnotations;

namespace FullFraim.Data.Base
{
    public class BaseEntity<T> : IModifiable, IBaseEntity<T>
    {
        [Key]
        public T Id { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
