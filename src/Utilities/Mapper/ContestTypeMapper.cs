using FullFraim.Data.Models;
using FullFraim.Models.Dto_s.ContestTypes;
using System.Linq;

namespace Utilities.Mapper
{
    public static class ContestTypeMapper
    {
        public static IQueryable<ContestTypeDto> MapToDto(this IQueryable<ContestType> query)
        {
            return query.Select(x =>
            new ContestTypeDto()
            {
                Id = x.Id,
                Name = x.Name
            });
        }

        public static ContestType MapToRaw(this ContestTypeDto model)
        {
            return new ContestType()
            {
                Id = model.Id,
                Name = model.Name,
            };
        }
    }
}
