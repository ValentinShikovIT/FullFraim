using FullFraim.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FullFraim.Data.Configurations
{
    public class PhaseConfig : IEntityTypeConfiguration<Phase>
    {
        public void Configure(EntityTypeBuilder<Phase> builder)
        {
            builder.HasQueryFilter(p => !p.IsDeleted);
        }
    }
}
