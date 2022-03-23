using FullFraim.Data.Models;
using FullFraim.Models.Dto_s.Reviews;
using FullFraim.Models.ViewModels.Contest;
using System.Linq;

namespace Utilities.Mapper
{
    public static class PhotoReviewMapper
    {
        public static OutputGiveReviewDto MapToOutputGiveReviewDto(this PhotoReview model, int contestId)
        {
            return new OutputGiveReviewDto()
            {
                Checkbox = model.Checkbox,
                Comment = model.Comment,
                JuryId = model.JuryContestId,
                PhotoId = model.PhotoId,
                ContestId = contestId,
                Score = model.Score,
            };
        }

        public static InputGiveReviewDto MapToInputGiveReviewDto(this GiveReviewViewModel model)
        {
            return new InputGiveReviewDto()
            {
                Checkbox = model.Review.Checkbox,
                Comment = model.Review.Comment,
                JuryId = model.Review.JuryId,
                ContestId = model.ContestId,
                PhotoId = model.Review.PhotoId,
                Score = model.Review.Score
            };
        }

        public static IQueryable<ReviewDto> MapToDto(this IQueryable<PhotoReview> model)
        {
            return model.Select(pr => new ReviewDto()
            {
                IsDisqualified = pr.Checkbox,
                AuthorName = pr.JuryContest.User.FirstName + " " + pr.JuryContest.User.LastName,
                Comment = pr.Comment,
                Score = pr.Score,
            });
        }
    }
}
