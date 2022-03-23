using FullFraim.Data;
using FullFraim.Data.Models;
using FullFraim.Models.Dto_s.Pagination;
using FullFraim.Models.Dto_s.PhotoJunkies;
using FullFraim.Models.Dto_s.Users;
using FullFraim.Services.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.AllConstants;
using System;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Mapper;

namespace FullFraim.Services.PhotoJunkieServices
{
    public class PhotoJunkieService : IPhotoJunkieService
    {
        private readonly FullFraimDbContext context;
        private readonly UserManager<User> userManager;

        public PhotoJunkieService(FullFraimDbContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task EnrollForContestAsync(InputEnrollForContestDto inputModel)
        {
            if (inputModel == null)
            {
                throw new NullModelException(string.Format(LogMessages.NullModel, "PhotoJunkieService", "EnrollForContestAsync"));
            }

            var participant = await this.context.ParticipantContests.FirstOrDefaultAsync(pc => pc.UserId == inputModel.UserId &&
                    pc.ContestId == inputModel.ContestId);

            // If currently participating without photo
            if (participant != null)
            {
                var photo = new Photo()
                {
                    Title = inputModel.ImageTitle,
                    Url = inputModel.PhotoUrl,
                    IsDeleted = false,
                    Story = inputModel.ImageDescription,
                    CreatedOn = DateTime.UtcNow,
                    ContestId = inputModel.ContestId,
                };

                var addedPhoto = await this.context.Photos.AddAsync(photo);

                await this.context.SaveChangesAsync();

                participant.PhotoId = addedPhoto.Entity.Id;
            }
            else
            {
                var toAddParticipantContest = new ParticipantContest()
                {
                    ContestId = inputModel.ContestId,
                    UserId = inputModel.UserId,
                    IsDeleted = false,
                    CreatedOn = DateTime.UtcNow,
                    Photo = new Photo()
                    {
                        Title = inputModel.ImageTitle,
                        Url = inputModel.PhotoUrl,
                        IsDeleted = false,
                        Story = inputModel.ImageDescription,
                        CreatedOn = DateTime.UtcNow,
                        ContestId = inputModel.ContestId,
                    }
                };

                await this.context.ParticipantContests.AddAsync(toAddParticipantContest);
            }

            await AddInitialPointsToUser(inputModel.ContestId, inputModel.UserId);
            await this.context.SaveChangesAsync();
        }

        public async Task<PaginatedModel<PhotoJunkyDto>> GetAllAsync(SortingModel sortingModel, PaginationFilter paginationFilter)
        {
            var users = this.context.Users.AsQueryable();

            users = sortingModel.OrderBy.ToLower() switch
            {
                Constants.Sorting.RankAsc => users.OrderBy(u => u.Rank),
                Constants.Sorting.RankDesc => users.OrderByDescending(u => u.Rank),
                Constants.Sorting.PointsAsc => users.OrderBy(u => u.Points),
                Constants.Sorting.PointsDesc => users.OrderByDescending(u => u.Points),
                Constants.Sorting.FirstNameAsc => users.OrderBy(u => u.FirstName),
                Constants.Sorting.FirstNameDesc => users.OrderByDescending(u => u.FirstName),
                Constants.Sorting.LastNameAsc => users.OrderBy(u => u.LastName),
                Constants.Sorting.LastNameDesc => users.OrderByDescending(u => u.LastName),

                _ => users
            };

            var paginatedModel = new PaginatedModel<PhotoJunkyDto>()
            {
                Model = await users
                    .Skip(paginationFilter.PageSize * (paginationFilter.PageNumber - 1))
                    .Take(paginationFilter.PageSize)
                    .MapToJunkieDto()
                    .ToListAsync(),
                RecordsPerPage = paginationFilter.PageSize,
                TotalPages = (int)Math.Ceiling(await this.context.Contests
                    .CountAsync(p => p.Id == p.Id) / (double)paginationFilter.PageSize),
            };

            return paginatedModel;
        }

        public async Task<bool> IsUserParticipant(int contestId, int userId)
        {
            if (contestId <= 0)
            {
                throw new InvalidIdException(string.Format(LogMessages.InvalidId, "PhotoJunkieService", "IsUserParticipant", contestId, "contest"));
            }

            if (userId <= 0)
            {
                throw new InvalidIdException(string.Format(LogMessages.InvalidId, "PhotoJunkieService", "IsUserParticipant", userId, "user"));
            }

            var isParticipant = await this.context.ParticipantContests
                .AnyAsync(p => p.UserId == userId && p.ContestId == contestId);

            return isParticipant;
        }

        public async Task<bool> IsUserJury(int contestId, int userId)
        {
            if (contestId <= 0)
            {
                throw new InvalidIdException(string.Format(LogMessages.InvalidId, "PhotoJunkieService", "IsUserJury", contestId, "contest"));
            }

            if (userId <= 0)
            {
                throw new InvalidIdException(string.Format(LogMessages.InvalidId, "PhotoJunkieService", "IsUserJury", userId, "user"));
            }

            var isJury = await this.context.JuryContests
                .AnyAsync(p => p.UserId == userId && p.ContestId == contestId);

            return isJury;
        }

        public async Task<PhotoJunkieRankDto> GetPointsTillNextRankAsync(int userId)
        {
            var user = await this.userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                throw new NotFoundException(string.Format(LogMessages.NotFound, "PhotoJunkieService", "GetPointsTillNextRankAsync", userId));
            }

            var junkieTillNextRankDto = new PhotoJunkieRankDto()
            {
                RankPoints = (int)user.Points,
                Rank = (await this.context.Ranks.Where(r => r.Id == user.RankId).FirstOrDefaultAsync()).Name,
                PointsTillNextRank = TillNextRankPoints((int)user.Points),
            };

            return junkieTillNextRankDto;
        }

        private int TillNextRankPoints(int currentPoints)
        {
            if (currentPoints <= 50)
            {
                return 51 - currentPoints;
            }
            else if (currentPoints <= 150)
            {
                return 151 - currentPoints;
            }
            else if (currentPoints <= 1000)
            {
                return 1001 - currentPoints;
            }

            return 0;
        }

        private async Task AddInitialPointsToUser(int contestId, int participantId)
        {
            string contestType = await this.context.Contests
                .Where(c => c.Id == contestId)
                .Select(c => c.ContestType.Name)
                .FirstOrDefaultAsync();

            var userToAddPoints = await this.context.Users
                .FirstOrDefaultAsync(u => u.Id == participantId);

            if (userToAddPoints == null)
            {
                throw new NotFoundException(string.Format(LogMessages.NotFoundModel, "PhotoJunkieService", "AddInitialPointsToUser", "User"));
            }

            userToAddPoints.Points += contestType switch
            {
                Constants.ContestType.Open => 1,
                Constants.ContestType.Invitational => 3,
                _ => throw new ArgumentException(string.Format(LogMessages.InvalidType, "PhotoJunkieService", "AddInitialPointsToUser")),
            };
        }

        public async Task<bool> HasCurrentUserSubmittedPhoto(int userId, int contestId)
        {
            return await this.context.ParticipantContests.AnyAsync(pc => pc.ContestId == contestId &&
                pc.UserId == userId && pc.PhotoId != null);
        }
    }
}
