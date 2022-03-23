using FullFraim.Models.Dto_s.Phases;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FullFraim.Services.PhaseServices
{
    public interface IPhaseService
    {
        Task<ICollection<PhaseDto>> GetAllAsync();
        Task<PhaseDto> GetByIdAsync(int id);
        Task<PhaseDto> CreateAsync(PhaseDto model);
        Task<PhaseDto> UpdateAsync(int id, PhaseDto model);
        Task DeleteAsync(int id);
    }
}