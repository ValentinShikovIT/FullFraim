using FullFraim.Models.Dto_s.Pagination;
using FullFraim.Models.Dto_s.PhotoJunkies;
using FullFraim.Models.Dto_s.Users;
using FullFraim.Services.PhotoJunkieServices;
using FullFraim.Web.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.AllConstants;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.CloudinaryUtils;
using Utilities.Mapper;

namespace FullFraim.Web.Controllers.ApiControllers
{
    [Authorize]
    [ApiController]
    [APIExceptionFilter]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class JunkiesController : ControllerBase
    {
        private readonly IPhotoJunkieService photoJunkieService;
        private readonly ICloudinaryUtils cloudinaryService;

        public JunkiesController(IPhotoJunkieService photoJunkieService,
            ICloudinaryUtils cloudinaryService)
        {
            this.photoJunkieService = photoJunkieService;
            this.cloudinaryService = cloudinaryService;
        }

        /// <summary>
        /// used to get all junkies
        /// </summary>
        /// <param name="sortingModel"></param>
        /// <param name="paginationFilter"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<PhotoJunkyDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll([FromQuery] SortingModel sortingModel, [FromQuery] PaginationFilter paginationFilter)
        {
            var junkies = await this.photoJunkieService.GetAllAsync(sortingModel, paginationFilter);

            return Ok(junkies);
        }

        /// <summary>
        /// used to enroll onto a given contest (Any data cannot be changed after submission.)
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        [HttpPost("enroll")]
        [IgnoreAntiforgeryToken]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Enroll([FromForm] InputEnrollForContestModel inputModel)
        {
            if (!(await this.photoJunkieService.IsUserParticipant(inputModel.ContestId, inputModel.UserId) &&
             await this.photoJunkieService.IsUserJury(inputModel.ContestId, inputModel.UserId)))
            {
                return BadRequest(error: string.Format(ErrorMessages.AlreadyInContest, inputModel.UserId, inputModel.ContestId));
            }

            var inputDto = inputModel.MapToDto();

            inputDto.PhotoUrl = this.cloudinaryService.UploadImage(inputModel.Photo);

            await this.photoJunkieService.EnrollForContestAsync(inputDto);

            return Ok();
        }

        /// <summary>
        /// used to get the points till next rank of the junkie
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("nextrank")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PhotoJunkyDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPointsTillNextRank([FromQuery] int userId)
        {
            if (userId < 0)
            {
                return BadRequest();
            }

            var junkieTillNextRankDto = await this.photoJunkieService.GetPointsTillNextRankAsync(userId);

            return Ok(junkieTillNextRankDto);
        }
    }
}
