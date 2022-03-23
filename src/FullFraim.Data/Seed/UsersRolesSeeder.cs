using FullFraim.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FullFraim.Data.Seed
{
    public class UsersRolesSeeder : ISeeder
    {
        public async Task SeedAsync(FullFraimDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            await this.SeedRoles(roleManager);
            await this.SeedAdmin(userManager, configuration);
            await this.SeedUsers(userManager);
        }

        private async Task SeedRoles(RoleManager<IdentityRole<int>> roleManager)
        {
            var roles = new List<string>()
            {
                Constants.Roles.Admin,
                Constants.Roles.Organizer,
                Constants.Roles.User,
            };

            foreach (var role in roles)
            {
                if (!(await roleManager.RoleExistsAsync(role)))
                    await roleManager.CreateAsync(new IdentityRole<int>(role));
            }
        }

        private async Task SeedUsers(UserManager<User> userManager)
        {
            var usersToSeed = new List<User>
            {
                new User()
                {
                    //Id = 2,
                    FirstName = Constants.UserData.Valentin,
                    LastName = Constants.UserData.Shikov,
                    UserName = Constants.UserData.VShikovEmail,
                    NormalizedUserName = Constants.UserData.VShikovEmail.ToUpper(),
                    Email = Constants.UserData.VShikovEmail,
                    NormalizedEmail = Constants.UserData.VShikovEmail.ToUpper(),
                    EmailConfirmed = true,
                    Points = 0,
                },
                new User()
                {
                    //Id = 3,
                    FirstName = Constants.UserData.Ivan,
                    LastName = Constants.UserData.Dichev,
                    UserName = Constants.UserData.IDichevEmail,
                    NormalizedUserName = Constants.UserData.IDichevEmail.ToUpper(),
                    Email = Constants.UserData.IDichevEmail,
                    NormalizedEmail = Constants.UserData.IDichevEmail.ToUpper(),
                    EmailConfirmed = true,
                    Points = 0,
                },
                new User()
                {
                    //Id = 4,
                    FirstName = Constants.UserData.Boryana,
                    LastName = Constants.UserData.Mihaylova,
                    UserName = Constants.UserData.BMihaylovaEmail,
                    NormalizedUserName = Constants.UserData.BMihaylovaEmail.ToUpper(),
                    Email = Constants.UserData.BMihaylovaEmail,
                    NormalizedEmail = Constants.UserData.BMihaylovaEmail.ToUpper(),
                    EmailConfirmed = true,
                    Points = 0,
                },
                new User()
                {
                    //Id = 5,
                    FirstName = Constants.UserData.Dimitar,
                    LastName = Constants.UserData.Dimitrov,
                    UserName = Constants.UserData.DDimitrovEmail,
                    NormalizedUserName = Constants.UserData.DDimitrovEmail.ToUpper(),
                    Email = Constants.UserData.DDimitrovEmail,
                    NormalizedEmail = Constants.UserData.DDimitrovEmail.ToUpper(),
                    EmailConfirmed = true,
                    Points = 0,
                },
                new User()
                {
                    //Id = 6,
                    FirstName = Constants.UserData.Emily,
                    LastName = Constants.UserData.Ivanova,
                    UserName = Constants.UserData.EIvanovaEmail,
                    NormalizedUserName = Constants.UserData.EIvanovaEmail.ToUpper(),
                    Email = Constants.UserData.EIvanovaEmail,
                    NormalizedEmail = Constants.UserData.EIvanovaEmail.ToUpper(),
                    EmailConfirmed = true,
                    Points = 0,
                },
            };

            foreach (var user in usersToSeed)
            {
                if (await userManager.FindByEmailAsync(user.Email) == null)
                {
                    var createdUserResult = await userManager.CreateAsync(user, "12345678901");

                    if (createdUserResult.Succeeded)
                    {
                        var createdUser = await userManager.FindByEmailAsync(user.Email);

                        await userManager.AddToRoleAsync(createdUser, Constants.Roles.User);
                    }
                }
            }
        }

        public async Task SeedAdmin(UserManager<User> userManager, IConfiguration configuration)
        {
            var admin = new User()
            {
                //Id = 1,
                FirstName = configuration["AccountAdminInfo:UserName"],
                LastName = configuration["AccountAdminInfo:LastName"],
                UserName = configuration["AccountAdminInfo:UserName"],
                NormalizedUserName = configuration["AccountAdminInfo:UserName"].ToUpper(),
                Email = configuration["AccountAdminInfo:Email"],
                NormalizedEmail = configuration["AccountAdminInfo:UserName"].ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
                Points = 0,
            };

            if (await userManager.FindByEmailAsync(admin.Email) == null)
            {
                var createdAdmin = await userManager.CreateAsync(admin, configuration["AccountAdminInfo:Password"]);

                if (createdAdmin.Succeeded)
                {
                    var createdUser = await userManager.FindByEmailAsync(admin.Email);

                    await userManager.AddToRoleAsync(createdUser, Constants.Roles.Organizer);
                    await userManager.AddToRoleAsync(createdUser, Constants.Roles.Admin);
                }
            }
        }
    }
}
