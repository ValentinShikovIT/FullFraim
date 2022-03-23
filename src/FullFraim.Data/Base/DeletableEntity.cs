using System;

namespace FullFraim.Data.Base
{
    public class DeletableEntity<T> : BaseEntity<T>, IDeletable
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
