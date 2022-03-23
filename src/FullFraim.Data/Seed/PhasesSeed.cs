using FullFraim.Data.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FullFraim.Data.Seed
{
    public class PhasesSeed : ISeeder
    {
        public static readonly List<Phase> SeedData = new List<Phase>()
        {
            new Phase()
            {
               Name = Constants.Phases.Finished
            },
            new Phase()
            {
               Name = Constants.Phases.PhaseII
            },
            new Phase()
            {
               Name = Constants.Phases.PhaseI
            },
        };

        public async Task SeedAsync(FullFraimDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.Phases.Any())
                await dbContext.AddRangeAsync(SeedData);
        }
    }
}
