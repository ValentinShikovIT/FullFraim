using FullFraim.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FullFraim.Data.Configurations
{
    public class ContestPhaseConfig : IEntityTypeConfiguration<ContestPhase>
    {
        public void Configure(EntityTypeBuilder<ContestPhase> builder)
        {
            builder.HasKey(cp => new { cp.PhaseId, cp.ContestId });

            builder.HasQueryFilter(cp => !cp.IsDeleted);
        }
    }
}
