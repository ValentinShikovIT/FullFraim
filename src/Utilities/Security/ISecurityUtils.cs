using System.Threading.Tasks;

namespace Utilities.Security
{
    public interface ISecurityUtils
    {
        Task<bool> IsUserJuryInContestAsync(int userId, int contestId);
        Task<bool> IsUserParticipantInContestAsync(int userId, int contestId);
        Task<bool> IsUserAdmin(int userId);
    }
}
