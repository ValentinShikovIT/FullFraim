using FullFraim.Models.Dto_s.ContestTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FullFraim.Services.ContestTypeServices
{
    public interface IContestTypeService
    {
        Task<ICollection<ContestTypeDto>> GetAllAsync();
        Task<ContestTypeDto> GetByIdAsync(int id);
        Task<ContestTypeDto> CreateAsync(ContestTypeDto model);
        Task<ContestTypeDto> UpdateAsync(int id, ContestTypeDto model);
        Task DeleteAsync(int id);
    }
}