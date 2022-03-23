using FullFraim.Models.Dto_s.ContestCategories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FullFraim.Services.ContestCatgeoryServices
{
    public interface IContestCategoryService
    {
        Task<ICollection<ContestCategoryDto>> GetAllAsync();
        Task<ContestCategoryDto> GetByIdAsync(int id);
        Task<ContestCategoryDto> CreateAsync(ContestCategoryDto model);
        Task<ContestCategoryDto> UpdateAsync(int id, ContestCategoryDto model);
        Task DeleteAsync(int id);
    }
}