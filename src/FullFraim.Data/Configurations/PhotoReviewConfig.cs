using FullFraim.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FullFraim.Data.Configurations
{
    public class PhotoReviewConfig : IEntityTypeConfiguration<PhotoReview>
    {
        public void Configure(EntityTypeBuilder<PhotoReview> builder)
        {
            builder.HasQueryFilter(pr => !pr.IsDeleted);
            builder.HasKey(pr => pr.Id);
            builder.HasIndex(pr => new { pr.PhotoId, pr.JuryContestId }).IsUnique();
        }
    }
}
