using FullFraim.Data.Models;
using FullFraim.Models.Dto_s.ContestCategories;
using System.Linq;

namespace Utilities.Mapper
{
    public static class ContestCategoryMapper
    {
        public static IQueryable<ContestCategoryDto> MapToDto(this IQueryable<ContestCategory> query)
        {
            return query.Select(x =>
                new ContestCategoryDto()
                {
                    Id = x.Id,
                    Name = x.Name
                }
            );
        }

        public static ContestCategory MapToRaw(this ContestCategoryDto model)
        {
            return new ContestCategory()
            {
                Id = model.Id,
                Name = model.Name,
            };
        }
    }
}

