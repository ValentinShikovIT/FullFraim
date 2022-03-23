using FullFraim.Data;
using FullFraim.Models.Dto_s.ContestCategories;
using FullFraim.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Shared.AllConstants;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Mapper;

namespace FullFraim.Services.ContestCatgeoryServices
{
    public class ContestCategoryService : IContestCategoryService
    {
        private readonly FullFraimDbContext context;

        public ContestCategoryService(FullFraimDbContext context)
        {
            this.context = context;
        }

        public async Task<ContestCategoryDto> CreateAsync(ContestCategoryDto model)
        {
            if (model == null)
            {
                throw new NullModelException(string.Format(LogMessages.NullModel, "ContestCategoryService", "CreateAsync"));
            }

            await this.context.ContestCategories
                .AddAsync(model.MapToRaw());

            await this.context
                .SaveChangesAsync();

            return model;
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new InvalidIdException(string.Format(LogMessages.InvalidId, "ContestCategoryService", "DeleteAsync", id, "ContestCategory"));
            }

            var modelToRemove = await this.context.ContestCategories
                .FirstOrDefaultAsync(CC => CC.Id == id);

            modelToRemove.DeletedOn = DateTime.UtcNow;
            modelToRemove.IsDeleted = true;

            await this.context.SaveChangesAsync();
        }

        public async Task<ICollection<ContestCategoryDto>> GetAllAsync()
        {
            var result = await this.context.ContestCategories
                .MapToDto()
                .ToListAsync();

            return result;
        }

        public async Task<ContestCategoryDto> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new InvalidIdException(string.Format(LogMessages.InvalidId, "ContestCategoryService", "GetByIdAsync()", id, "ContestCategory"));
            }

            var result = await this.context.ContestCategories
                .MapToDto()
                .FirstOrDefaultAsync(CC => CC.Id == id);

            if (result == null)
            {
                throw new NotFoundException(string.Format(LogMessages.NotFound, "ContestCategoryService", "GetByIdAsync()", id));
            }

            return result;
        }

        public async Task<ContestCategoryDto> UpdateAsync(int id, ContestCategoryDto model)
        {
            if (model == null)
            {
                throw new NullModelException(string.Format(LogMessages.NullModel, "ContestCategoryService", "UpdateAsync()"));
            }

            if (id <= 0)
            {
                throw new InvalidIdException(string.Format(LogMessages.InvalidId, "ContestCategoryService", "UpdateAsync()", id, "ContestCategory"));
            }

            var dbModelToUpdate = await this.context.ContestCategories
                .FirstOrDefaultAsync(cc => cc.Id == id);

            if (dbModelToUpdate == null)
            {
                throw new NotFoundException(string.Format(LogMessages.NotFound, "ContestCategoryService", "UpdateAsync()", id));
            }

            dbModelToUpdate.Name = model.Name ?? dbModelToUpdate.Name;
            dbModelToUpdate.ModifiedOn = DateTime.UtcNow;

            await this.context.SaveChangesAsync();

            return model;
        }
    }
}
