using FullFraim.Data;
using FullFraim.Data.Models;
using FullFraim.Models.ViewModels.Dashboard;
using FullFraim.Services.PhotoJunkieServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Utilities.Mapper;

namespace FullFraim.Web.ViewComponents
{
    public class PointsTillNextViewComponent : ViewComponent
    {
        private readonly IPhotoJunkieService photoJunkieService;
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;
        private readonly FullFraimDbContext context;

        public PointsTillNextViewComponent
            (IPhotoJunkieService photoJunkieService,
            UserManager<User> userManager,
            IConfiguration configuration,
            FullFraimDbContext context)
        {
            this.photoJunkieService = photoJunkieService;
            this.userManager = userManager;
            this.configuration = configuration;
            this.context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var junkies = await userManager.Users
                .Where(x => x.FirstName != configuration["AccountAdminInfo:UserName"])
                .OrderByDescending(r => r.Points)
                .Take(5)
                .ToListAsync();

            var result = new List<RankAndPointsViewModel>();

            foreach (var junkie in junkies)
            {
                var photojunkieWithPoints = await photoJunkieService
                    .GetPointsTillNextRankAsync(junkie.Id);

                result
                    .Add(photojunkieWithPoints
                    .MapToPointsViewModel
                    ($"{junkie.FirstName} {junkie.LastName}"));
            }

            result = result.ToList();

            ViewBag.CurrentUser = await GetCurrentUserPointsAsync();

            return View(result);
        }

        private async Task<CurrentUserViewModel> GetCurrentUserPointsAsync()
        {
            var id = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var user = await this.context.Users
                .Where(x => x.Id == id)
                .Select(x => new { x.FirstName, x.LastName, x.Points })
                .FirstOrDefaultAsync();

            var pointsTillNext = await photoJunkieService
                    .GetPointsTillNextRankAsync(id);

            var result = new CurrentUserViewModel()
            {
                CurrentPoints = (int)user.Points,
                Ranking = pointsTillNext.MapToPointsViewModel($"{user.FirstName} {user.LastName}"),
                Rank = pointsTillNext.Rank,
            };

            return result;
        }
    }
}
