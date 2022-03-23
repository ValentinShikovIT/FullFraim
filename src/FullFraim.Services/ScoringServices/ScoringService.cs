using FullFraim.Data;
using FullFraim.Models.Dto_s.Scorings;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FullFraim.Services.ScoringServices
{
    public class ScoringService : IScoringService
    {
        private const string FirstWinners = "firstWinners";
        private const string SecondWinners = "secondWinners";
        private const string ThirdWinners = "thirdWinners";

        private readonly FullFraimDbContext context;

        public ScoringService(FullFraimDbContext context)
        {
            this.context = context;
        }

        public async Task AwardWinnersAsync(int contestId)                        // Once in Phase III
        {
            var currentContest = await this.context.Contests
                .Include(c => c.ParticipantContests)
                .FirstOrDefaultAsync(c => c.Id == contestId);

            var photoReviews = await this.context.PhotoReviews.ToListAsync();
            var photos = await this.context.Photos.Include(x => x.Participant).ToListAsync();
            var users = await this.context.Users.ToListAsync();

            var allParticipants = currentContest.ParticipantContests;

            if (allParticipants.Count == 0)                                       // Checks if there are no participants in the contest
            {
                return;
            }

            var allParticipantsDto = new List<InputScoringDto>();                 // Lists all participants with their calculated score

            foreach (var participant in allParticipants)
            {
                var currentParticipant = new InputScoringDto()
                {
                    UserId = participant.UserId,
                    Score = CalculateUserScore(participant.UserId, participant.ContestId)
                };

                allParticipantsDto.Add(currentParticipant);
            }

            var topThreeScores = allParticipantsDto                                // Finds top 3 scores and orders by descending (eg. 9.5, 8, 7)
                .Select(p => p.Score)
                .ToHashSet()
                .OrderByDescending(p => p)
                .Take(3)
                .ToList();

            var prizeWinnerDict = new Dictionary<string, List<InputScoringDto>>();  // Holds list of winners for each prize

            prizeWinnerDict[FirstWinners] = allParticipantsDto.Where(p => p.Score == topThreeScores[0]).ToList();

            if (topThreeScores.Count > 1)                                           // Checks if there are more than one participants
            {
                prizeWinnerDict[SecondWinners] = allParticipantsDto.Where(p => p.Score == topThreeScores[1]).ToList();
            }

            if (topThreeScores.Count > 2)                                          // Checks if there are more than two participants
            {
                prizeWinnerDict[ThirdWinners] = allParticipantsDto.Where(p => p.Score == topThreeScores[2]).ToList();
            }

            UpdateWinnersPoints(prizeWinnerDict, topThreeScores);                  // Updates winners points in database

            await this.context.SaveChangesAsync();
        }

        private void UpdateWinnersPoints(Dictionary<string, List<InputScoringDto>> prizeWinnerDict, List<double> topThreeScores)
        {
            foreach (var kvp in prizeWinnerDict)       // Checks how many winners for each prize: first, second and third
            {
                if (kvp.Key == FirstWinners)
                {
                    foreach (var winner in kvp.Value)  // Finds all the first winners and updates their points
                    {
                        var firstWinner = this.context.ParticipantContests.Include(pc => pc.User).FirstOrDefault(pc => pc.UserId == winner.UserId);

                        if (kvp.Value.Count == 1)
                        {
                            firstWinner.User.Points += 50;

                            if (FirstScoreDoublesTheSecond(topThreeScores))
                            {
                                firstWinner.User.Points += 25;
                            }
                        }
                        else
                        {
                            firstWinner.User.Points += 40;

                            if (FirstScoreDoublesTheSecond(topThreeScores))
                            {
                                firstWinner.User.Points += 35;
                            }
                        }
                    }
                }
                else if (kvp.Key == SecondWinners)
                {
                    foreach (var winner in kvp.Value)  // Finds all the second winners and updates their points
                    {
                        var secondWinner = this.context.ParticipantContests.FirstOrDefault(pc => pc.UserId == winner.UserId);

                        if (kvp.Value.Count == 1)
                        {
                            secondWinner.User.Points += 35;
                        }
                        else
                        {
                            secondWinner.User.Points += 25;
                        }
                    }
                }
                else
                {
                    foreach (var winner in kvp.Value)  // Finds all the third winners and updates their points
                    {
                        var thirdWinner = this.context.ParticipantContests.FirstOrDefault(pc => pc.UserId == winner.UserId);

                        if (kvp.Value.Count == 1)
                        {
                            thirdWinner.User.Points += 20;
                        }
                        else
                        {
                            thirdWinner.User.Points += 10;
                        }
                    }
                }
            }
        }

        private bool FirstScoreDoublesTheSecond(List<double> topThreeScores)
        {
            if (topThreeScores.Count > 1 && topThreeScores[0] >= 2 * topThreeScores[1])
            {
                return true;
            }

            return false;
        }

        private double CalculateUserScore(int userId, int contestId)
        {
            var userReviews = this.context.PhotoReviews                      // Finds all the reviews for the current user' photo
                .Where(pr => pr.Photo.Participant.UserId == userId &&
                pr.Photo.Participant.ContestId == contestId).ToList();

            int userReviewsCount = userReviews.Count();

            double userFinalScore;

            if (userReviewsCount > 0)
            {
                var userScoreSum = userReviews.Sum(pr => pr.Score);
                userFinalScore = userScoreSum / (double)userReviewsCount;
            }
            else
            {
                userFinalScore = 3;                                          // If a photo is not reviewed, a default score of 3 is awarded
            }

            return userFinalScore;
        }
    }
}
