using FullFraim.Data;
using FullFraim.Models.Dto_s.Phases;
using FullFraim.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Shared.AllConstants;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Mapper;

namespace FullFraim.Services.PhaseServices
{
    public class PhaseService : IPhaseService
    {
        private readonly FullFraimDbContext context;

        public PhaseService(FullFraimDbContext context)
        {
            this.context = context;
        }

        public async Task<PhaseDto> CreateAsync(PhaseDto model)
        {
            if (model == null)
            {
                throw new NullModelException(string.Format(LogMessages.NullModel, "PhaseService", "CreateAsync"));
            }

            await this.context.Phases
                .AddAsync(model.MapToRaw());

            await this.context
                .SaveChangesAsync();

            return model;
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new InvalidIdException(string.Format(LogMessages.InvalidId, "PhaseService", "DeleteAsync", id, "phase"));
            }

            var modelToRemove = await this.context.Phases
                .FirstOrDefaultAsync(CC => CC.Id == id);

            modelToRemove.DeletedOn = DateTime.UtcNow;
            modelToRemove.IsDeleted = true;

            await this.context.SaveChangesAsync();
        }

        public async Task<ICollection<PhaseDto>> GetAllAsync()
        {
            var result = await this.context.Phases
                .MapToDto()
                .ToListAsync();

            return result;
        }

        public async Task<PhaseDto> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new InvalidIdException(string.Format(LogMessages.InvalidId, "PhaseService", "GetByIdAsync", id, "phase"));
            }

            var result = await this.context.Phases
                .MapToDto()
                .FirstOrDefaultAsync(CC => CC.Id == id);

            if (result == null)
            {
                throw new NotFoundException(string.Format(LogMessages.NotFound, "PhaseService", "GetByIdAsync", id));
            }

            return result;
        }

        public async Task<PhaseDto> UpdateAsync(int id, PhaseDto model)
        {
            if (model == null)
            {
                throw new NullModelException(string.Format(LogMessages.NullModel, "PhaseService", "UpdateAsync"));
            }

            if (id <= 0)
            {
                throw new InvalidIdException(string.Format(LogMessages.InvalidId, "PhaseService", "UpdateAsync", id, "phase"));
            }

            var dbModelToUpdate = await this.context.Phases
                .FirstOrDefaultAsync(CC => CC.Id == id);

            if (dbModelToUpdate == null)
            {
                throw new NotFoundException(string.Format(LogMessages.NotFound, "PhaseService", "UpdateAsync", id));
            }

            dbModelToUpdate.Name = model.Name ?? dbModelToUpdate.Name;
            dbModelToUpdate.ModifiedOn = DateTime.UtcNow;

            await this.context.SaveChangesAsync();

            return model;
        }
    }
}
