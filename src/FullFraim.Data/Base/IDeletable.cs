using System;

namespace FullFraim.Data.Base
{
    public interface IDeletable
    {
        bool IsDeleted { get; set; }
        DateTime? DeletedOn { get; set; }
    }
}
