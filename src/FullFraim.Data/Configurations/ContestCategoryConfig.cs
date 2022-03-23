using FullFraim.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FullFraim.Data.Configurations
{
    public class ContestCategoryConfig : IEntityTypeConfiguration<ContestCategory>
    {
        public void Configure(EntityTypeBuilder<ContestCategory> builder)
        {
            builder.HasQueryFilter(cc => !cc.IsDeleted);
        }
    }
}
