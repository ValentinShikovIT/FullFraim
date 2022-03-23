using Microsoft.AspNetCore.Http;

namespace FullFraim.Models.Dto_s.PhotoJunkies
{
    public class InputEnrollForContestDto
    {
        public int UserId { get; set; }

        public int ContestId { get; set; }

        public string PhotoUrl { get; set; }

        public IFormFile Photo { get; set; }

        public string ImageDescription { get; set; }

        public string ImageTitle { get; set; }
    }
}
