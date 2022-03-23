using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Utilities.Security;

namespace FullFraim.Web.Controllers
{
    [Authorize]
    public abstract class BaseMvcController : Controller
    {
        private readonly ISecurityUtils securityUtils;

        public BaseMvcController()
        {
        }

        public BaseMvcController(ISecurityUtils securityService)
        {
            this.securityUtils = securityService;
        }

        public int UserId { get => int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value); }

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
    }
}
