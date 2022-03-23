using FullFraim.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FullFraim.Data.Configurations
{
    public class PhotoConfig : IEntityTypeConfiguration<Photo>
    {
        public void Configure(EntityTypeBuilder<Photo> builder)
        {
            builder.HasQueryFilter(p => !p.IsDeleted);

            builder.HasOne(p => p.Contest)
                .WithMany(c => c.Photos)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
