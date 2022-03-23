using FullFraim.Data;
using FullFraim.Data.Models;
using FullFraim.Models.Dto_s.Contests;
using FullFraim.Models.Dto_s.Pagination;
using FullFraim.Models.Dto_s.User;
using FullFraim.Services.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.AllConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Mapper;

namespace FullFraim.Services.ContestServices
{
    public class ContestService : IContestService
    {
        private readonly FullFraimDbContext context;
        private readonly UserManager<User> userManager;

        public ContestService(FullFraimDbContext context,
            UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<bool> IsNameUniqueAsync(string name)
        {
            var isUnique = (await this.context.Contests.FirstOrDefaultAsync(c => c.Name == name) == null);

            return isUnique;
        }

        public async Task<OutputContestDto> CreateAsync(InputContestDto model)
        {
            if (model == null)
            {
                throw new NullModelException(string.Format(LogMessages.NullModel, "ContestService", "CreateAsync"));
            }

            model.Phases.StartDate_PhaseI = DateTime.UtcNow;
            model.Phases.StartDate_PhaseII = model.Phases.EndDate_PhaseI;
            model.Phases.StartDate_PhaseIII = model.Phases.EndDate_PhaseII;
            model.Phases.EndDate_PhaseIII = DateTime.MaxValue;

            var contest = await this.context.Contests.AddAsync(model.MapToRaw());

            await this.context.SaveChangesAsync();

            await AddOrganizersToJuryContest(contest.Entity.Id);

            // If contest is invitational - invite users to join
            if (model.ContestTypeId ==
                (await this.context.ContestTypes.FirstOrDefaultAsync(ct => ct.Name == Constants.ContestType.Invitational)).Id)
            {
                await this.AddInvitedForTheContestAsync(model.Jury, model.Participants, contest.Entity.Id);
            }

            await this.AddContestPhasesAsync(model, contest.Entity.Id);

            await this.context.SaveChangesAsync();

            return this.context.Contests.Where(c => c.Id == contest.Entity.Id).MapToDto().FirstOrDefault();
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new InvalidIdException(string.Format(LogMessages.InvalidId, "ContestService", "DeleteAsync", id, "contest"));
            }

            var modelToRemove = await this.context.Contests
                .FirstOrDefaultAsync(cc => cc.Id == id);

            if (modelToRemove == null)
            {
                throw new NotFoundException(string.Format(LogMessages.NotFound, "ContestService", "DeleteAsync", id));
            }

            modelToRemove.DeletedOn = DateTime.UtcNow;
            modelToRemove.IsDeleted = true;

            await this.context.SaveChangesAsync();
        }

        public async Task<PaginatedModel<OutputContestDto>> GetAllAsync
            (int? participantId, int? juryId, string phase, string contestType, PaginationFilter paginationFilter)
        {
            var contests = this.context.Contests.AsQueryable();

            if (participantId != null || juryId != null)
            {
                contests = contests.Where(c => c.ParticipantContests.Any(pc => pc.UserId == participantId) ||
                    c.JuryContests.Any(jc => jc.UserId == juryId));
            }

            if (!string.IsNullOrEmpty(phase))
            {
                contests = contests.Where(c => c.ContestPhases.Any(cp => cp.Phase.Name == phase &&
                    cp.EndDate > DateTime.UtcNow && cp.StartDate < DateTime.UtcNow));
            }

            if (!string.IsNullOrEmpty(contestType))
            {
                contests = contests.Where(c => c.ContestType.Name == contestType);
            }

            var paginatedModel = new PaginatedModel<OutputContestDto>()
            {
                Model = await contests.OrderByDescending(c => c.CreatedOn)
                    .Skip(paginationFilter.PageSize * (paginationFilter.PageNumber - 1))
                    .Take(paginationFilter.PageSize)
                    .MapToDto()
                    .ToListAsync(),
                RecordsPerPage = paginationFilter.PageSize,
                TotalPages = (int)Math.Ceiling(await this.context.Contests
                    .CountAsync(p => p.Id == p.Id) / (double)paginationFilter.PageSize),
            };

            return paginatedModel;
        }

        public async Task<PaginatedModel<OutputContestDto>> GetAllForUserByPhaseAsync
            (int userId, PaginationFilter paginationFilter, int categoryId, string phase)
        {
            var contests = this.context.Contests.AsQueryable();

            contests = contests.Where(c => c.ParticipantContests.Any(pc => pc.UserId == userId) ||
                c.JuryContests.Any(jc => jc.UserId == userId) ||
                    (c.ContestPhases.Any(cp => cp.Phase.Name == Constants.Phases.PhaseI &&
                    cp.EndDate > DateTime.UtcNow && cp.StartDate < DateTime.UtcNow) &&
                c.ContestType.Name == Constants.ContestType.Open));

            contests = contests.Where(c => c.ContestPhases.Any(cp => cp.Phase.Name == phase &&
                    cp.EndDate > DateTime.UtcNow && cp.StartDate < DateTime.UtcNow));

            if (categoryId > 0)
            {
                contests = contests.Where(c => c.ContestCategoryId == categoryId);
            }

            var paginatedModel = new PaginatedModel<OutputContestDto>()
            {
                Model = await contests.OrderByDescending(c => c.CreatedOn)
                    .Skip(paginationFilter.PageSize * (paginationFilter.PageNumber - 1))
                    .Take(paginationFilter.PageSize)
                    .MapToDto(userId)
                    .ToListAsync(),
                RecordsPerPage = paginationFilter.PageSize,
                TotalPages = (int)Math.Ceiling(await contests
                    .CountAsync(p => p.Id == p.Id) / (double)paginationFilter.PageSize),
            };

            return paginatedModel;
        }

        public async Task<PaginatedModel<string>> GetContestCoversAsync(PaginationFilter paginationFilter)
        {
            var paginatedModel = new PaginatedModel<string>()
            {
                Model = await this.context.Contests.OrderByDescending(c => c.CreatedOn)
                   .Skip(paginationFilter.PageSize * (paginationFilter.PageNumber - 1))
                   .Take(paginationFilter.PageSize)
                   .MapToUrl()
                   .ToListAsync(),
                RecordsPerPage = paginationFilter.PageSize,
                TotalPages = (int)Math.Ceiling(await this.context.Contests
                   .CountAsync(p => p.Id == p.Id) / (double)paginationFilter.PageSize),
            };

            return paginatedModel;
        }

        public async Task<OutputContestDto> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new InvalidIdException(string.Format(LogMessages.InvalidId, "ContestService", "GetByIdAsync", id, "contest"));
            }

            var result = await this.context.Contests
                .Where(c => c.Id == id)
                .MapToDto()
                .FirstOrDefaultAsync();

            if (result == null)
            {
                throw new NotFoundException(string.Format(LogMessages.NotFound, "ContestService", "GetByIdAsync", id));
            }

            return result;
        }

        public async Task UpdateAsync(int id, InputContestDto model)
        {
            if (model == null)
            {
                throw new NullModelException(string.Format(LogMessages.NullModel, "ContestService", "UpdateAsync"));
            }

            var dbModelToUpdate = await this.context.Contests
                .FirstOrDefaultAsync(CC => CC.Id == id);

            if (dbModelToUpdate == null)
            {
                throw new NotFoundException(string.Format(LogMessages.NotFound, "ContestService", "UpdateAsync", id));
            }

            if (this.context.Contests.Any(c => c.Name == model.Name))
            {
                throw new UniqueNameException(string.Format(LogMessages.UniqueName, "ContestService", "UpdateAsync", model.Name));
            }

            dbModelToUpdate.Name = model.Name ?? dbModelToUpdate.Name;
            dbModelToUpdate.ModifiedOn = DateTime.UtcNow;

            await this.context.SaveChangesAsync();
        }

        public async Task<ICollection<UserDto>> GetParticipantsForInvitationAsync()
        {
            var users = await userManager.GetUsersInRoleAsync(Constants.Roles.User);

            var usersDto = users.MapToDto();

            return usersDto;
        }

        public async Task<ICollection<UserDto>> GetPotentialJuryForInvitationAsync()
        {
            var users = await userManager.GetUsersInRoleAsync(Constants.Roles.User);

            users = users
                .Where(u => u.Points >= 150)
                .ToList();

            return users.MapToDto();
        }

        private async Task AddContestPhasesAsync(InputContestDto model, int id)
        {
            await this.context.ContestPhases
               .AddAsync(new ContestPhase()
               {
                   ContestId = id,
                   PhaseId = 1,
                   StartDate = model.Phases.StartDate_PhaseI,
                   EndDate = model.Phases.EndDate_PhaseI,
               });

            await this.context.ContestPhases
                .AddAsync(new ContestPhase()
                {
                    ContestId = id,
                    PhaseId = 2,
                    StartDate = model.Phases.StartDate_PhaseII,
                    EndDate = model.Phases.EndDate_PhaseII,
                });

            await this.context.ContestPhases
              .AddAsync(new ContestPhase()
              {
                  ContestId = id,
                  PhaseId = 3,
                  StartDate = model.Phases.StartDate_PhaseIII,
                  EndDate = model.Phases.EndDate_PhaseIII,
              });
        }

        private async Task AddOrganizersToJuryContest(int contestId)
        {
            var organisers = await userManager.GetUsersInRoleAsync(Constants.Roles.Organizer);

            if (organisers.Count == 0)
            {
                throw new NoOrganizersException
                    (string.Format(LogMessages.NotFoundModel, "ContestService", "AddOrganizersToJuryContest", "organizer"));
            }

            foreach (var organizer in organisers)
            {
                var juryContest = new JuryContest()
                {
                    UserId = organizer.Id,
                    ContestId = contestId,
                };

                await this.context.JuryContests.AddAsync(juryContest);
            }
        }

        private async Task AddInvitedForTheContestAsync
            (ICollection<int> jury, ICollection<int> participants, int contestId)
        {
            if (participants == null || jury == null)
            {
                throw new NullModelException(string.Format(LogMessages.NullModel, "ContestService", "AddInvitedForTheContestAsync", contestId));
            }

            if (participants.Any(p => jury.Any(j => j == p)))
            {
                //Funny Joke :D
                throw new CheaterException($"{DateTime.UtcNow} Cannot invite participant to be jury!");
            }

            var contest = await this.context.Contests
                .FirstOrDefaultAsync(c => c.Id == contestId);

            if (contest == null)
            {
                throw new NotFoundException(string.Format(LogMessages.NotFound, "ContestService", "AddInvitedForTheContestAsync", contestId));
            }

            var juryContests = jury.MapToJuryContest(contestId);
            var participantContests = participants.MapToParticipantContest(contestId);

            foreach (var juryCont in juryContests)
            {
                await this.context.JuryContests.AddAsync(juryCont);
            }

            foreach (var partCont in participantContests)
            {
                await this.context.ParticipantContests.AddAsync(partCont);
            }

            await this.context.SaveChangesAsync();
        }

        public async Task<bool> IsContestInPhaseFinished(int contestId)
        {
            if (contestId <= 0)
            {
                throw new InvalidIdException
                    (string.Format(LogMessages.InvalidId, "ContestService", "AddInvitedForTheContestAsync", contestId, "contest"));
            }

            return await this.context.Contests
                .Where(c => c.Id == contestId)
                .Where(c => c.ContestPhases.Any(cp => cp.Phase.Name == Constants.Phases.Finished &&
                    cp.StartDate < DateTime.UtcNow && cp.EndDate > DateTime.UtcNow)).AnyAsync();
        }
    }
}
