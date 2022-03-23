using FullFraim.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FullFraim.Data.Configurations
{
    public class ContestTypeConfig : IEntityTypeConfiguration<ContestType>
    {
        public void Configure(EntityTypeBuilder<ContestType> builder)
        {
            builder.HasQueryFilter(ct => !ct.IsDeleted);
        }
    }
}
