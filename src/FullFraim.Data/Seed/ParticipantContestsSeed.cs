using FullFraim.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FullFraim.Data.Seed
{
    public class ParticipantContestsSeed : ISeeder
    {
        public static readonly List<ParticipantContest> SeedData = new List<ParticipantContest>()
        {
            // PhaseOne contest
            new ParticipantContest()
            {
                ContestId = 1,
                UserId = 2,
                PhotoId = 1,
            },
            new ParticipantContest()
            {
                ContestId = 1,
                UserId = 3,
                PhotoId = 2,
            },
            new ParticipantContest()
            {
                ContestId = 1,
                UserId = 4,
                PhotoId = 3,
            },
            new ParticipantContest()
            {
                ContestId = 1,
                UserId = 5,
                PhotoId = 4,
            },
            // PhaseTwo contest
            new ParticipantContest()
            {
                ContestId = 2,
                UserId = 2,
                PhotoId = 5,
            },
            new ParticipantContest()
            {
                ContestId = 2,
                UserId = 3,
                PhotoId = 6,
            },
            new ParticipantContest()
            {
                ContestId = 2,
                UserId = 4,
                PhotoId = 7,
            },
            new ParticipantContest()
            {
                ContestId = 2,
                UserId = 5,
                PhotoId = 8,
            },
            // Finished contest
            new ParticipantContest()
            {
                ContestId = 3,
                UserId = 2,
                PhotoId = 9,
            },
            new ParticipantContest()
            {
                ContestId = 3,
                UserId = 3,
                PhotoId = 10,
            },
            new ParticipantContest()
            {
                ContestId = 3,
                UserId = 4,
                PhotoId = 11,
            },
            new ParticipantContest()
            {
                ContestId = 3,
                UserId = 5,
                PhotoId = 12,
            },
            // Portraits contest
            new ParticipantContest()
            {
                ContestId = 4,
                UserId = 6,
                PhotoId = 13,
            },
        };

        public async Task SeedAsync(FullFraimDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.ParticipantContests.Any())
                await dbContext.AddRangeAsync(SeedData);
        }
    }
}
