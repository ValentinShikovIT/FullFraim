using FullFraim.Data.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FullFraim.Data.Seed
{
    public class RanksSeed : ISeeder
    {
        public static readonly List<Rank> SeedData = new List<Rank>()
        {
            new Rank()
            {
                Name = Constants.Ranks.Junkie,
            },
            new Rank()
            {
                Name = Constants.Ranks.Enthusiast,
            },
            new Rank()
            {
                Name = Constants.Ranks.Master,
            },
            new Rank()
            {
                Name = Constants.Ranks.WiseAndBenevolentPhotoDictator,
            },
        };

        public async Task SeedAsync(FullFraimDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.Ranks.Any())
                await dbContext.AddRangeAsync(SeedData);
        }
    }
}
