using FullFraim.Models.Dto_s.Reviews;
using System.Threading.Tasks;

namespace FullFraim.Services.JuryServices
{
    public interface IJuryService
    {
        Task<OutputGiveReviewDto> GiveReviewAsync(InputGiveReviewDto inputModel);
        Task<bool> IsContestInPhaseTwoAsync(int photoId);
        Task<bool> HasJuryAlreadyGivenReviewAsync(int juryId, int photoId);
        Task<bool> IsJuryGivenReviewForPhotoAsync(int photoId, int juryId);
        Task<bool> IsUserJuryForContest(int contestId, int juryId);
        Task<ReviewDto> GetReviewAsync(int juryId, int photoId);
    }
}
