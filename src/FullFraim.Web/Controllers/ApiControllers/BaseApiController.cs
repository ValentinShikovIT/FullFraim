using Utilities.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FullFraim.Web.Controllers.ApiControllers
{
    [ApiController]
    [Authorize(Roles = Constants.Roles.Admin)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public abstract class BaseApiController : ControllerBase
    {
        private readonly ISecurityUtils securityUtils;

        public BaseApiController()
        {
        }

        public BaseApiController(ISecurityUtils securityService)
        {
            this.securityUtils = securityService;
        }

        protected internal async Task<bool> IsCurrentUserJuryInContestAsync(int contestId)
        {
            var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            return await this.securityUtils.IsUserJuryInContestAsync(userId, contestId);
        }

        protected internal async Task<bool> IsCurrentUserParticipantInContestAsync(int contestId)
        {
            var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            return await this.securityUtils.IsUserParticipantInContestAsync(userId, contestId);
        }

        protected internal async Task<bool> IsUserAdmin()
        {
            var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            return await this.securityUtils.IsUserAdmin(userId);
        }
    }
}
