using FullFraim.Models.Dto_s.Pagination;
using FullFraim.Models.Dto_s.Photos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FullFraim.Services.PhotoService
{
    public interface IPhotoService
    {
        Task<PhotoDto> GetByIdAsync(int photoId);
        Task<PaginatedModel<PhotoDto>> GetPhotosForContestAsync
            (int userid, int contestId, PaginationFilter paginationFilter);
        Task<ICollection<PhotoDto>> GetTopRecentPhotosAsync();
        Task<bool> IsPhotoSubmitedByUserAsync(int userId, int photoId);
        Task<PaginatedModel<ContestSubmissionOutputDto>> GetDetailedSubmissionsFromContestAsync
            (int contestId, PaginationFilter paginationFilter);
        Task<PaginatedModel<ContestSubmissionOutputDto>> GetDetailedSubmissionsFromContestAsync
            (int contestId, PaginationFilter paginationFilter, int juryContestId);
        Task<PhotoDto> GetUserSubmissionForContestAsync(int userid, int contestId);
        Task<PaginatedModel<ContestSubmissionOutputDto>> GetDetailedSubmissionsForPhoto
            (int contestId, int photoId, PaginationFilter paginationFilter);
    }
}
