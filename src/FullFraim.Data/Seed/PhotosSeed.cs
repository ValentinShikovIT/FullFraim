using FullFraim.Data.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FullFraim.Data.Seed
{
    public class PhotosSeed : ISeeder
    {
        public static readonly List<Photo> SeedData = new List<Photo>()
        {
            new Photo()
            {
                ContestId = 1,
                Title = "Squirrel",
                Story = "Looking down",
                Url = Constants.Images.WildlifeImgUrl,
            },
            new Photo()
            {
                ContestId = 1,
                Title = "Bath time",
                Story = "On my way",
                Url = Constants.Images.WildlifeImg2Url,
            },
            new Photo()
            {
                ContestId = 1,
                Title = "Fight in the night",
                Story = "Subway fighters",
                Url = Constants.Images.WildlifeImg3Url,
            },
            new Photo()
            {
                ContestId = 1,
                Title = "I can climb it",
                Story = "Not a long way, we can climb it",
                Url = Constants.Images.WildlifeImg4Url,
            },
            new Photo()
            {
                ContestId = 2,
                Title = "Fight in the night",
                Story = "Subway fighters",
                Url = Constants.Images.WildlifeImg3Url,
            },
            new Photo()
            {
                ContestId = 2,
                Title = "I can climb it",
                Story = "Not a long way, we can climb it",
                Url = Constants.Images.WildlifeImg4Url,
            },
            new Photo()
            {
                ContestId = 2,
                Title = "Can I have some?",
                Story = "Hungry birds",
                Url = Constants.Images.WildlifeImg5Url,
            },
            new Photo()
            {
                ContestId = 2,
                Title = "Git It!",
                Story = "Got it!",
                Url = Constants.Images.WildlifeImg6Url,
            },

            new Photo()
            {
                ContestId = 3,
                Title = "Squirrel",
                Story = "Looking down",
                Url = Constants.Images.WildlifeImgUrl,
            },
            new Photo()
            {
                ContestId = 3,
                Title = "Bath time",
                Story = "On my way",
                Url = Constants.Images.WildlifeImg2Url,
            },
            new Photo()
            {
                ContestId = 3,
                Title = "Fight in the night",
                Story = "Subway fighters",
                Url = Constants.Images.WildlifeImg3Url,
            },
            new Photo()
            {
                ContestId = 3,
                Title = "I can climb it",
                Story = "Not a long way, we can climb it",
                Url = Constants.Images.WildlifeImg4Url,
            },
            new Photo()
            {
                ContestId = 4,
                Title = "Smile",
                Story = "Just a nice picture",
                Url = Constants.Images.PortraitImgUrlCover,
            }
        };

        public async Task SeedAsync(FullFraimDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.Photos.Any())
                await dbContext.AddRangeAsync(SeedData);
        }
    }
}
