using FullFraim.Data;
using FullFraim.Models.Dto_s.ContestTypes;
using FullFraim.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Shared.AllConstants;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Mapper;

namespace FullFraim.Services.ContestTypeServices
{
    public class ContestTypeService : IContestTypeService
    {
        private readonly FullFraimDbContext context;

        public ContestTypeService(FullFraimDbContext context)
        {
            this.context = context;
        }

        public async Task<ContestTypeDto> CreateAsync(ContestTypeDto model)
        {
            if (model == null)
            {
                throw new NullModelException(string.Format(LogMessages.NullModel, "ContestTypeService", "CreateAsync"));
            }

            await this.context.ContestTypes
                .AddAsync(model.MapToRaw());

            await this.context
                .SaveChangesAsync();

            return model;
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new InvalidIdException(string.Format(LogMessages.InvalidId, "ContestTypeService", "DeleteAsync", id, "contest type"));
            }

            var modelToRemove = await this.context.ContestTypes
                .FirstOrDefaultAsync(CC => CC.Id == id);

            modelToRemove.DeletedOn = DateTime.UtcNow;
            modelToRemove.IsDeleted = true;

            await this.context.SaveChangesAsync();
        }

        public async Task<ICollection<ContestTypeDto>> GetAllAsync()
        {
            var result = await this.context.ContestTypes
                .MapToDto()
                .ToListAsync();

            return result;
        }

        public async Task<ContestTypeDto> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new InvalidIdException(string.Format(LogMessages.InvalidId, "ContestTypeService", "GetByIdAsync", id, "contest type"));
            }

            var result = await this.context.ContestTypes
                .MapToDto()
                .FirstOrDefaultAsync(CC => CC.Id == id);

            if (result == null)
            {
                throw new NotFoundException(string.Format(LogMessages.NotFound, "ContestTypeService", "GetByIdAsync", id));
            }

            return result;
        }

        public async Task<ContestTypeDto> UpdateAsync(int id, ContestTypeDto model)
        {
            if (model == null)
            {
                throw new NullModelException(string.Format(LogMessages.NullModel, "ContestTypeService", "UpdateAsync"));
            }

            if (id <= 0)
            {
                throw new InvalidIdException(string.Format(LogMessages.InvalidId, "ContestTypeService", "UpdateAsync", id, "contest type"));
            }

            var dbModelToUpdate = await this.context.ContestTypes
                .FirstOrDefaultAsync(cc => cc.Id == id);

            if (dbModelToUpdate == null)
            {
                throw new NotFoundException(string.Format(LogMessages.NotFound, "ContestTypeService", "UpdateAsync", id));
            }

            dbModelToUpdate.Name = model.Name ?? dbModelToUpdate.Name;
            dbModelToUpdate.ModifiedOn = DateTime.UtcNow;

            await this.context.SaveChangesAsync();

            return model;
        }
    }
}
