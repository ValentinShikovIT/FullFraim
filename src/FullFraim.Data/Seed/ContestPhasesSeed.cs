using FullFraim.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FullFraim.Data.Seed
{
    public class ContestPhasesSeed : ISeeder
    {
        public readonly List<ContestPhase> SeedData = new List<ContestPhase>()
        {
            // Contest PhaseOne
            new ContestPhase()
            {
                ContestId = 1,
                PhaseId = 1,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(30),
            },
            new ContestPhase()
            {
                ContestId = 1,
                PhaseId = 2,
                StartDate = DateTime.UtcNow.AddDays(30),
                EndDate = DateTime.UtcNow.AddDays(60),
            },
            new ContestPhase()
            {
                ContestId = 1,
                PhaseId = 3,
                StartDate = DateTime.UtcNow.AddDays(90),
                EndDate = DateTime.MaxValue,
            },
            // Contest PhaseTwo
            new ContestPhase()
            {
                ContestId = 2,
                PhaseId = 1,
                StartDate = DateTime.UtcNow.AddDays(-30),
                EndDate = DateTime.UtcNow,
            },
            new ContestPhase()
            {
                ContestId = 2,
                PhaseId = 2,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(30),
            },
            new ContestPhase()
            {
                ContestId = 2,
                PhaseId = 3,
                StartDate = DateTime.UtcNow.AddDays(30),
                EndDate = DateTime.MaxValue,
            },
            // Contest Finished
            new ContestPhase()
            {
                ContestId = 3,
                PhaseId = 1,
                StartDate = DateTime.UtcNow.AddDays(-2),
                EndDate = DateTime.UtcNow.AddDays(-1),
            },
            new ContestPhase()
            {
                ContestId = 3,
                PhaseId = 2,
                StartDate = DateTime.UtcNow.AddDays(-1),
                EndDate = DateTime.UtcNow,
            },
            new ContestPhase()
            {
                ContestId = 3,
                PhaseId = 3,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.MaxValue,
            },
            new ContestPhase()
            {
                ContestId = 4,
                PhaseId = 1,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(30),
            },
            new ContestPhase()
            {
                ContestId = 4,
                PhaseId = 2,
                StartDate = DateTime.UtcNow.AddDays(30),
                EndDate = DateTime.UtcNow.AddDays(60),
            },
            new ContestPhase()
            {
                ContestId = 4,
                PhaseId = 3,
                StartDate = DateTime.UtcNow.AddDays(60),
                EndDate = DateTime.MaxValue,
            },
        };

        public async Task SeedAsync(FullFraimDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.ContestPhases.Any())
                await dbContext.AddRangeAsync(SeedData);
        }
    }
}
