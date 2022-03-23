using FullFraim.Models.Dto_s.Reviews;
using FullFraim.Services.JuryServices;
using FullFraim.Web.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.AllConstants;
using System.Threading.Tasks;

namespace FullFraim.Web.Controllers.ApiControllers
{
    [Authorize]
    [ApiController]
    [APIExceptionFilter]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class JuriesController : Controller
    {
        private readonly IJuryService juryService;

        public JuriesController(IJuryService juryService)
        {
            this.juryService = juryService;
        }

        /// <summary>
        /// used to get all reviews of one jury
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        [HttpPost("review")]
        [IgnoreAntiforgeryToken]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OutputGiveReviewDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GiveReview([FromBody] InputGiveReviewDto inputModel)
        {
            var juryId = inputModel.JuryId;
            var photoId = inputModel.PhotoId;

            if (!await juryService.IsContestInPhaseTwoAsync(photoId))
            {
                return BadRequest(new { ErrorMsg = ErrorMessages.ReviewOutsidePhaseTwo });
            }

            if (await juryService.HasJuryAlreadyGivenReviewAsync(juryId, photoId))
            {
                return BadRequest(new { ErrorMsg = ErrorMessages.ReviewAlreadyGiven });
            }

            var toAddReview = await this.juryService.GiveReviewAsync(inputModel);

            return Ok(toAddReview);
        }
    }
}
