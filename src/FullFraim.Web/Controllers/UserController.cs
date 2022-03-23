using FullFraim.Data.Models;
using FullFraim.Models.Dto_s.Pagination;
using FullFraim.Models.ViewModels.Dashboard;
using FullFraim.Models.ViewModels.Sorting;
using FullFraim.Models.ViewModels.User;
using FullFraim.Services.PhotoJunkieServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Mapper;
using static Shared.Constants;

namespace FullFraim.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IPhotoJunkieService photoJunkieService;
        private readonly UserManager<User> userManager;
        private static readonly List<SortingViewModel> sortingCollection = new List<SortingViewModel>()
        {
            new SortingViewModel()
            {
                ViewName = Sorting.FirstNameAscView,
                ServerName = Sorting.FirstNameAsc
            },
            new SortingViewModel()
            {
                ViewName = Sorting.FirstNameDescView,
                ServerName = Sorting.FirstNameDesc
            },
            new SortingViewModel()
            {
                ViewName = Sorting.LastNameAscView,
                ServerName = Sorting.LastNameAsc
            },
            new SortingViewModel()
            {
                ViewName = Sorting.LastNameDescView,
                ServerName = Sorting.LastNameDesc
            },
            new SortingViewModel()
            {
                ViewName = Sorting.RankAscView,
                ServerName = Sorting.RankAsc
            },
            new SortingViewModel()
            {
                ViewName = Sorting.RankDescView,
                ServerName = Sorting.RankDesc
            },
            new SortingViewModel()
            {
                ViewName = Sorting.PointsAscView,
                ServerName = Sorting.PointsAsc
            },
            new SortingViewModel()
            {
                ViewName = Sorting.PointsDescView,
                ServerName = Sorting.PointsDesc
            }
        };

        public UserController(IConfiguration configuration,
            IPhotoJunkieService photoJunkieService,
            UserManager<User> userManager)
        {
            this.configuration = configuration;
            this.photoJunkieService = photoJunkieService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index([FromQuery] string orderBy = "", [FromQuery] int pageNumber = 1)
        {
            var paginationFilter = new PaginationFilter() { PageNumber = pageNumber };

            var junkies = await this.photoJunkieService
                .GetAllAsync(new SortingModel() { OrderBy = orderBy },
                paginationFilter);

            var result = new List<RankAndPointsViewModel>();

            foreach (var junkie in junkies.Model)
            {
                var photojunkieWithPoints = await photoJunkieService
                    .GetPointsTillNextRankAsync(junkie.Id);

                result
                    .Add(photojunkieWithPoints
                    .MapToPointsViewModel
                    (junkie.FirstName + junkie.LastName));
            }

            ViewBag.Sorting = sortingCollection;

            var ViewResult = new UsersPageViewModel()
            {
                Sorting = orderBy,
                Model = result,
                PaginatedModel = junkies,
                PageFilter = paginationFilter,
            };

            return View(ViewResult);
        }
    }
}
