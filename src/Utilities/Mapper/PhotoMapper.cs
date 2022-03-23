using FullFraim.Data.Models;
using FullFraim.Models.Dto_s.Phases;
using FullFraim.Models.Dto_s.Photos;
using FullFraim.Models.Dto_s.Reviews;
using FullFraim.Models.ViewModels.Home;
using System.Linq;

namespace Utilities.Mapper
{
    public static class PhotoMapper
    {
        public static IQueryable<PhotoDto> MapToDto(this IQueryable<Photo> query)
        {
            return query.Select(p => new PhotoDto()
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Story,
                SubmitterName = $"{p.Participant.User.FirstName} {p.Participant.User.LastName}",
                Url = p.Url,
            });
        }

        public static IQueryable<ContestSubmissionOutputDto> MapToContestSubmissionOutputDto
            (this IQueryable<Photo> query)
        {
            return query.Select(p => new ContestSubmissionOutputDto()
            {
                ContestName = p.Contest.Name,
                ContestCategory = p.Contest.ContestCategory.Name,
                contestId = p.ContestId,
                PhotoId = p.Id,
                AuthorName = $"{p.Participant.User.FirstName} {p.Participant.User.LastName}",
                AuthorId = p.Participant.UserId,
                PhotoTitle = p.Title,
                PhotoUrl = p.Url,
                Score = p.PhotoReviews.Sum(pr => pr.Score) / (double)p.PhotoReviews.Count(),
                Description = p.Story,
                PhasesInfo = p.Contest.ContestPhases.Select(y => new PhaseDto()
                {
                    Name = y.Phase.Name,
                    StartDate = y.StartDate,
                    EndDate = y.EndDate
                }).ToList(),
                Reviews = p.PhotoReviews.Select(pr => new ReviewDto()
                {
                    AuthorName = $"{pr.JuryContest.User.FirstName} {pr.JuryContest.User.LastName}",
                    Comment = pr.Comment,
                    ReviewId = pr.Id, 
                    Score = pr.Score,
                }).ToList(),
            });
        }

        public static IQueryable<ContestSubmissionOutputDto> MapToContestSubmissionOutputDto
            (this IQueryable<Photo> query,
            int userId)
        {
            return query.Select(p => new ContestSubmissionOutputDto()
            {
                contestId = p.ContestId,
                PhotoId = p.Id,
                AuthorName = $"{p.Participant.User.FirstName} {p.Participant.User.LastName}",
                AuthorId = p.Participant.UserId,
                PhotoTitle = p.Title,
                PhotoUrl = p.Url,
                Score = p.PhotoReviews.Sum(pr => pr.Score) / p.PhotoReviews.Count(),
                Description = p.Story,
                PhasesInfo = p.Contest.ContestPhases.Select(y => new PhaseDto()
                {
                    Name = y.Phase.Name,
                    StartDate = y.StartDate,
                    EndDate = y.EndDate
                }).ToList(),
                Reviews = p.PhotoReviews.Select(pr => new ReviewDto()
                {
                    AuthorName = $"{pr.JuryContest.User.FirstName} {pr.JuryContest.User.LastName}",
                    Comment = pr.Comment,
                    ReviewId = pr.Id, 
                    Score = pr.Score,
                }).ToList(),
                HasJuryGivenReview = p.PhotoReviews
                .Any(pr => pr.JuryContest.UserId == userId && pr.PhotoId == p.Id)
            });
        }

        public static PhotoReview MapToRaw(ReviewDto model)
        {
            return new PhotoReview()
            {
                Checkbox = model.IsDisqualified,
                Comment = model.Comment,
                PhotoId = model.PhotoId,
                Score = model.Score
            };
        }

        public static HomeIndexViewModel MapToHomeViewModel(this PhotoDto model)
        {
            return new HomeIndexViewModel()
            {
                PhotoUrl = model.Url,
                Description = model.Description,
                SubmitterName = model.SubmitterName,
                Title = model.Title,
            };
        }

        public static UserSubmissionViewModel MapToUserSubmissionViewModel(this PhotoDto model)
        {
            return new UserSubmissionViewModel()
            {
                PhotoUrl = model.Url,
                Description = model.Description,
                SubmitterName = model.SubmitterName,
                Title = model.Title,
            };
        }
    }
}

