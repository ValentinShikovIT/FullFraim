using FullFraim.Models.Dto_s.AccountAPI;
using Utilities.API_JwtService;
using FullFraim.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FullFraim.Web.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IJwtServices jwtServices;

        public AccountController(IJwtServices jwtServices)
        {
            this.jwtServices = jwtServices;
        }

        /// <summary>
        /// used to login and receive the JWT Token
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public async Task<IActionResult> Login([FromQuery] InputLoginModel_API model)
        {
            var result = await this.jwtServices.Login(model);

            if (result != null)
            {
                return Ok(result);
            }

            return Unauthorized();
        }

        /// <summary>
        /// used to register in our system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [ServiceFilter(typeof(APIExceptionFilter))]
        public async Task<IActionResult> Register([FromBody] RegisterInputModel_API model)
        {
            if (ModelState.IsValid)
            {
                var result = await this.jwtServices.Register(model);

                if (result == true)
                {
                    return Ok("Registered");
                }
            }

            return BadRequest();
        }
    }
}
