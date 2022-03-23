using FullFraim.Data.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FullFraim.Data.Seed
{
    public class ContestsSeed : ISeeder
    {
        public static readonly List<Contest> SeedData = new List<Contest>()
        {
            new Contest()
            {
                Name = "Portrait",
                Description = "Portraits and photos of groups or individuals.",
                Cover_Url = Constants.Images.PortraitImgUrlCover,
                ContestCategoryId = 10,
                ContestTypeId = 2,
                CreatedOn = DateTime.UtcNow,
            },
            new Contest()
            {
                Name = "Wild life",
                Description = "Photos of non-domestic animals.",
                Cover_Url = Constants.Images.WildlifeImgUrlCover,
                ContestCategoryId = 12,
                ContestTypeId = 1,
                CreatedOn = DateTime.UtcNow.AddDays(-2),
            },
            new Contest()
            {
                Name = "Dynamic fashion",
                Description = "Photos of people of different age showing a fashion detail.",
                Cover_Url = Constants.Images.WildlifeImgUrlCover,
                ContestCategoryId = 12,
                ContestTypeId = 1,
                CreatedOn = DateTime.UtcNow.AddDays(-30),
            },
            new Contest()
            {
                Name = "Abstract",
                Description = "Abstract photos.",
                Cover_Url = Constants.Images.WildlifeImgUrlCover,
                ContestCategoryId = 12,
                ContestTypeId = 1,
                CreatedOn = DateTime.UtcNow,
            },
            new Contest()
            {
                Name = "Architecture",
                Description = "Photos of different kind of buildings.",
                Cover_Url = Constants.Images.WildlifeImgUrlCover,
                ContestCategoryId = 12,
                ContestTypeId = 1,
                CreatedOn = DateTime.UtcNow,
            },
            new Contest()
            {
                Name = "Conceptual",
                Description = "Conceptual art.",
                Cover_Url = Constants.Images.WildlifeImgUrlCover,
                ContestCategoryId = 12,
                ContestTypeId = 1,
                CreatedOn = DateTime.UtcNow,
            },
            new Contest()
            {
                Name = "Fine Art",
                Description = "Fine art photos.",
                Cover_Url = Constants.Images.WildlifeImgUrlCover,
                ContestCategoryId = 12,
                ContestTypeId = 1,
                CreatedOn = DateTime.UtcNow,
            },
            new Contest()
            {
                Name = "Nature",
                Description = "Photos of nature..",
                Cover_Url = Constants.Images.WildlifeImgUrlCover,
                ContestCategoryId = 12,
                ContestTypeId = 1,
                CreatedOn = DateTime.UtcNow,
            },
             new Contest()
            {
                Name = "Photo Journalism",
                Description = "Brave photo journalism photos.",
                Cover_Url = Constants.Images.WildlifeImgUrlCover,
                ContestCategoryId = 12,
                ContestTypeId = 1,
                CreatedOn = DateTime.UtcNow,
            },
             new Contest()
            {
                Name = "Street",
                Description = "Photos from the street.",
                Cover_Url = Constants.Images.WildlifeImgUrlCover,
                ContestCategoryId = 12,
                ContestTypeId = 1,
                CreatedOn = DateTime.UtcNow,
            },
        };

        public async Task SeedAsync(FullFraimDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.Contests.Any())
                foreach (var contest in SeedData)
                {
                    await dbContext.AddAsync(contest);
                }
        }
    }
}
