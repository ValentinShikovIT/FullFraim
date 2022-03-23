using FullFraim.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FullFraim.Data.Seed
{
    public class JuryContestsSeed : ISeeder
    {
        public static readonly List<JuryContest> SeedData = new List<JuryContest>()
        {
            new JuryContest()
            {
                ContestId = 1,
                UserId = 1,
            },
            new JuryContest()
            {
                ContestId = 2,
                UserId = 1,
            },
            new JuryContest()
            {
                ContestId = 3,
                UserId = 1,
            },
            new JuryContest()
            {
                ContestId = 4,
                UserId = 1,
            },
        };

        public async Task SeedAsync(FullFraimDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.JuryContests.Any())
                await dbContext.AddRangeAsync(SeedData);
        }
    }
}
