using FullFraim.Data.Models;
using FullFraim.Models.Dto_s.AccountAPI;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.API_JwtService
{
    public class JWTUtils : IJwtServices
    {
        private readonly IOptions<JwtSettings> options;
        private readonly UserManager<User> userManager;

        public JWTUtils(IOptions<JwtSettings> options,
            UserManager<User> userManager)
        {
            this.options = options;
            this.userManager = userManager;
        }

        public async Task<OutputLoginModel_API> Login(InputLoginModel_API model)
        {
            var userName = model.Username;
            var password = model.Password;

            User user = await userManager.FindByNameAsync(userName);

            if (user == null ||
                !await userManager.CheckPasswordAsync(user, password))
            {
                return null;
            }

            var secret = this.options.Value.Secret;
            var key = Encoding.UTF8.GetBytes(secret);

            var userRoles = await userManager.GetRolesAsync(user);

            var roleClaims = userRoles.Select(ur => new Claim(ClaimTypes.Role, ur));
            var authClaims = new List<Claim>(roleClaims)
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = new JwtSecurityToken(
                    expires: DateTime.Now.AddHours(24),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
                    );

            var jwt = tokenHandler.WriteToken(token);

            return new OutputLoginModel_API() { Username = userName, JwtToken = jwt, Id = user.Id };
        }

        public async Task<bool> Register(RegisterInputModel_API model)
        {
            var email = model.Email;
            var password = model.Password;

            var user = new User { UserName = email, Email = email };
            var result = await userManager.CreateAsync(user, password);

            await this.userManager
                .AddToRoleAsync(user, Constants.Roles.User);

            return result.Succeeded;
        }
    }
}
