using System.Threading.Tasks;

namespace FullFraim.Services.ScoringServices
{
    public interface IScoringService
    {
        Task AwardWinnersAsync(int contestId);
    }
}
