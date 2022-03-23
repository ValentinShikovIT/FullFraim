using FullFraim.Models.Dto_s.Contests;
using FullFraim.Models.Dto_s.Pagination;
using FullFraim.Services.ContestServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FullFraim.Web.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DashboardController : ControllerBase
    {
        private readonly IContestService contestService;

        public DashboardController(IContestService contestService)
        {
            this.contestService = contestService;
        }

        /// <summary>
        /// used to get contest without additional information
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="categoryId"></param>
        /// <param name="paginationFilter"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<OutputContestDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll([FromQuery] int userId, string phase, int categoryId, [FromQuery] PaginationFilter paginationFilter)
        {
            if (userId <= 0)
            {
                return BadRequest();
            }

            var allForUser = await this.contestService.GetAllForUserByPhaseAsync(userId, paginationFilter, categoryId, phase);

            return Ok(allForUser);
        }
    }
}
