using FullFraim.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FullFraim.Data.Seed
{
    public class PhotoReviewsSeed : ISeeder
    {
        public static readonly List<PhotoReview> SeedData = new List<PhotoReview>()
        {
            // Phase One - Cannot be given to the photos
            new PhotoReview()
            {
                JuryContestId = 1,
                PhotoId = 1,
                Score = 4,
                Checkbox = false,
                Comment = "nice",
            },
            new PhotoReview()
            {
                JuryContestId = 1,
                PhotoId = 2,
                Score = 10,
                Comment = "Extraordinary",
                Checkbox = false,
            },
            new PhotoReview()
            {
                JuryContestId = 1,
                PhotoId = 3,
                Score = 6,
                Comment = "nice",
                Checkbox = false,
            },
            new PhotoReview()
            {
                JuryContestId = 1,
                PhotoId = 4,
                Score = 6,
                Comment = "nice",
                Checkbox = false,
            },
            new PhotoReview()
            {
                JuryContestId = 1,
                PhotoId = 5,
                Score = 8,
                Comment = "nice",
                Checkbox = false,
            },
            new PhotoReview()
            {
                JuryContestId = 1,
                PhotoId = 6,
                Score = 4,
                Comment = "nice",
                Checkbox = false,
            },
            new PhotoReview()
            {
                JuryContestId = 1,
                PhotoId = 7,
                Score = 8,
                Comment = "nice",
                Checkbox = false,
            },
            new PhotoReview()
            {
                JuryContestId = 1,
                PhotoId = 8,
                Score = 5,
                Comment = "nice",
                Checkbox = false,
            },
        };

        public async Task SeedAsync(FullFraimDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.PhotoReviews.Any())
                await dbContext.AddRangeAsync(SeedData);
        }
    }
}
