using FullFraim.Data.Models;
using FullFraim.Models.Contest.ViewModels;
using FullFraim.Models.Dto_s.Contests;
using FullFraim.Models.Dto_s.Phases;
using FullFraim.Models.Dto_s.Photos;
using FullFraim.Models.Dto_s.User;
using FullFraim.Models.ViewModels.Contest;
using FullFraim.Models.ViewModels.Dashboard;
using System.Collections.Generic;
using System.Linq;

namespace Utilities.Mapper
{
    public static class ContestMapper
    {
        public static InputContestDto MapToDto(this CreateContestViewModel model)
        {
            return new InputContestDto()
            {
                Name = model.Name,
                Cover_Url = model.Cover_Url,
                Description = model.Description,
                ContestCategoryId = model.ContestCategoryId,
                ContestTypeId = model.ContestTypeId,
                Phases = model.Phases,
                Jury = model.Juries,
                Participants = model.Participants,
            };
        }

        public static Contest MapToRaw(this InputContestDto model)
        {
            return new Contest()
            {
                Name = model.Name,
                Cover_Url = model.Cover_Url,
                Description = model.Description,
                ContestCategoryId = model.ContestCategoryId,
                ContestTypeId = model.ContestTypeId,
            };
        }

        public static DashboardViewModel MapToViewDashboard(this OutputContestDto model)  // this ContestDto model
        {
            return new DashboardViewModel()
            {
                ContestId = model.Id,
                Name = model.Name,
                CategoryName = model.ContestCategory,
                Cover_Url = model.Cover_Url,
                Description = model.Description,
                ContestCategory = model.ContestCategoryId,
                ActivePhase = model.ActivePhase,
                IsCurrentUserParticipant = model.IsCurrentUserParticipant,
                IsCurrentUserJury = model.IsCurrentUserJury,
            };
        }

        public static ContestSubmissionViewModel MapToContestSubmissionView(this ContestSubmissionOutputDto model)
        {
            return new ContestSubmissionViewModel()
            {
                ContestName = model.ContestName,
                ContestCategory = model.ContestCategory,
                PhotoId = model.PhotoId,
                ContestId = model.contestId,
                AuthorId = model.AuthorId,
                AuthorName = model.AuthorName,
                Image_Url = model.PhotoUrl,
                Description = model.Description,
                Score = model.Score,
                Reviews = model.Reviews,
                ActivePhase = model.ActivePhase,
                HasJuryGivenSubmission = model.HasJuryGivenReview
            };
        }

        public static IQueryable<OutputContestDto> MapToDto(this IQueryable<Contest> query)
        {
            return query.Select(x =>
            new OutputContestDto()
            {
                Id = x.Id,
                Name = x.Name,
                Cover_Url = x.Cover_Url,
                Description = x.Description,
                ContestCategoryId = x.ContestCategoryId,
                ContestTypeId = x.ContestTypeId,
                ContestTypeName = x.ContestType.Name,
                ContestCategory = x.ContestCategory.Name,
                PhasesInfo = x.ContestPhases.Select(y => new PhaseDto()
                {
                    Name = y.Phase.Name,
                    StartDate = y.StartDate,
                    EndDate = y.EndDate
                }).ToList()
            });
        }

        public static IQueryable<OutputContestDto> MapToDto(this IQueryable<Contest> query, int userId)
        {
            return query.Select(x =>
            new OutputContestDto()
            {
                Id = x.Id,
                Name = x.Name,
                Cover_Url = x.Cover_Url,
                Description = x.Description,
                ContestCategoryId = x.ContestCategoryId,
                ContestTypeId = x.ContestTypeId,
                ContestCategory = x.ContestCategory.Name,
                IsCurrentUserJury = x.JuryContests.Any(x => x.UserId == userId),
                IsCurrentUserParticipant = x.ParticipantContests.Any(x => x.UserId == userId),
                PhasesInfo = x.ContestPhases.Select(y => new PhaseDto()
                {
                    Name = y.Phase.Name,
                    StartDate = y.StartDate,
                    EndDate = y.EndDate
                }).ToList()
            });
        }

        public static IQueryable<string> MapToUrl(this IQueryable<Contest> query)
        {
            return query
                .Select(x => x.Cover_Url);
        }

        public static IQueryable<UserDto> MapToDto(this IQueryable<User> query)
        {
            return query.Select(q => new UserDto()
            {
                UserId = q.Id,
                FirstName = q.FirstName,
                LastName = q.LastName,
                RankId = q.RankId,
                Points = q.Points
            });
        }

        public static ICollection<JuryContest> MapToJuryContest
            (this ICollection<int> users, int contestId)
        {
            var list = new List<JuryContest>();

            foreach (var user in users)
            {
                var juryContest = new JuryContest()
                {
                    ContestId = contestId,
                    UserId = user,
                };

                list.Add(juryContest);
            }

            return list.ToList();
        }

        public static ICollection<ParticipantContest> MapToParticipantContest
            (this ICollection<int> users, int contestId)
        {
            var list = new List<ParticipantContest>();

            foreach (var user in users)
            {
                var participantContest = new ParticipantContest()
                {
                    ContestId = contestId,
                    UserId = user,
                };

                list.Add(participantContest);
            }

            return list;
        }

        public static ICollection<UserDto> MapToDto
            (this ICollection<User> users)
        {
            var list = new List<UserDto>();

            foreach (var user in users)
            {
                var userDto = new UserDto()
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    RankId = user.RankId,
                    Points = user.Points
                };

                list.Add(userDto);
            }

            return list.ToList();
        }
    }
}
