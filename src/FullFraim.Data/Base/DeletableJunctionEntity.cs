using System;

namespace FullFraim.Data.Base
{
    public class DeletableJunctionEntity : BaseJunctionEntity, IDeletable
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
