using FullFraim.Data.Models;
using FullFraim.Models.Dto_s.Phases;
using System.Collections.Generic;
using System.Linq;

namespace Utilities.Mapper
{
    public static class PhaseMapper
    {
        public static IQueryable<PhaseDto> MapToDto(this IQueryable<Phase> query)
        {
            return query.Select(x =>
            new PhaseDto()
            {
                Id = x.Id,
                Name = x.Name
            });
        }

        public static ICollection<PhaseDto> MapToDto(this ICollection<Phase> collection)
        {
            var listPhaseDto = new List<PhaseDto>();

            foreach (var item in collection)
            {
                listPhaseDto.Add(
                    new PhaseDto()
                    {
                        Id = item.Id,
                        Name = item.Name
                    });
            }

            return listPhaseDto;
        }

        public static Phase MapToRaw(this PhaseDto model)
        {
            return new Phase()
            {
                Id = model.Id,
                Name = model.Name,
            };
        }
    }
}
