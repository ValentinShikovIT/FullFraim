using FullFraim.Models.Dto_s.PhotoJunkies;
using FullFraim.Models.ViewModels.Enrolling;

namespace Utilities.Mapper
{
    public static class EnrollMapper
    {
        public static InputEnrollForContestDto MapToDto(this InputEnrollForContestModel model)
        {
            return new InputEnrollForContestDto()
            {
                ContestId = model.ContestId,
                ImageDescription = model.ImageDescription,
                ImageTitle = model.ImageTitle,
                UserId = model.UserId,
            };
        }

        public static InputEnrollForContestDto MapToDto(this EnrollViewModel model,
            string photoUrl)
        {
            return new InputEnrollForContestDto()
            {
                UserId = model.UserId,
                ContestId = model.ContestId,
                PhotoUrl = photoUrl,
                ImageTitle = model.ImageTitle,
                ImageDescription = model.ImageDescription
            };
        }
    }
}
