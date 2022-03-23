using FullFraim.Data.Base;
using System.Collections.Generic;

namespace FullFraim.Data.Models
{
    public class Rank : DeletableEntity<int>
    {
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
