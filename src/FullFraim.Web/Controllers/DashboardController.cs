using FullFraim.Models.Dto_s.Pagination;
using FullFraim.Models.Dto_s.Reviews;
using FullFraim.Models.ViewModels.Contest;
using FullFraim.Models.ViewModels.Dashboard;
using FullFraim.Models.ViewModels.Enrolling;
using FullFraim.Services.ContestCatgeoryServices;
using FullFraim.Services.ContestServices;
using FullFraim.Services.JuryServices;
using FullFraim.Services.PhotoJunkieServices;
using FullFraim.Services.PhotoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.AllConstants;
using System.Linq;
using System.Threading.Tasks;
using Utilities.CloudinaryUtils;
using Utilities.Mapper;

namespace FullFraim.Web.Controllers
{
    [Authorize]
    public class DashboardController : BaseMvcController
    {
        private readonly IContestService contestService;
        private readonly IContestCategoryService contestCategoryService;
        private readonly IPhotoJunkieService photoJunkieService;
        private readonly IJuryService juryService;
        private readonly IPhotoService photoService;
        private readonly ICloudinaryUtils cloudinaryService;

        public DashboardController(IContestService contestService,
            IContestCategoryService contestCategoryService,
            IPhotoJunkieService photoJunkieService,
            IJuryService juryService,
            IPhotoService photoService,
            ICloudinaryUtils cloudinaryService)
        {
            this.contestService = contestService;
            this.contestCategoryService = contestCategoryService;
            this.photoJunkieService = photoJunkieService;
            this.juryService = juryService;
            this.photoService = photoService;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<IActionResult> Index(int categoryId, PaginationFilter paginationFilter)
        {
            // Default page size for this page
            paginationFilter.PageSize = 4;

            var dashboardPaginatedViewModel = new DashboardPhasesPaginatedViewModel()
            {
                PhaseOne = await GetContestViewModelByPhaseAsync(categoryId, paginationFilter, Constants.Phases.PhaseI),
                PhaseTwo = await GetContestViewModelByPhaseAsync(categoryId, paginationFilter, Constants.Phases.PhaseII),
                Finished = await GetContestViewModelByPhaseAsync(categoryId, paginationFilter, Constants.Phases.Finished),
            };

            dashboardPaginatedViewModel.PhaseOne.CurrentPage = paginationFilter.PageNumber;
            dashboardPaginatedViewModel.PhaseTwo.CurrentPage = paginationFilter.PageNumber;
            dashboardPaginatedViewModel.Finished.CurrentPage = paginationFilter.PageNumber;

            ViewBag.Categories = await this.contestCategoryService.GetAllAsync();

            return View(dashboardPaginatedViewModel);
        }

        public async Task<IActionResult> ContestPhaseNavigation(int categoryId, PaginationFilter paginationFilter, string phase)
        {
            paginationFilter.PageSize = 4;

            var contests = await this.GetContestViewModelByPhaseAsync(categoryId, paginationFilter, phase);
            contests.CurrentPage = paginationFilter.PageNumber;

            return PartialView("~/Views/Shared/Partials/_ContestCardPartial.cshtml", contests);
        }

        public IActionResult Enroll(int contestId)
        {
            return PartialView("~/Views/Shared/Partials/_EnrollPartial.cshtml",
                new EnrollViewModel() { ContestId = contestId });
        }

        [HttpPost]
        public async Task<IActionResult> Enroll(EnrollViewModel model)
        {
            model.UserId = UserId;

            // Cannot enroll if user is jury for contest
            if (await this.photoJunkieService.IsUserJury(model.ContestId, this.UserId))
            {
                ModelState
                    .AddModelError(string.Empty,
                    errorMessage: ErrorMessages.CannotEnroll);
            }

            if (!ModelState.IsValid)
            {
                return PartialView("~/Views/Shared/Partials/_EnrollPartial.cshtml", model);
            }

            string imageUrl = this.cloudinaryService
                .UploadImage(model.Photo);

            await photoJunkieService
                .EnrollForContestAsync(model.MapToDto(imageUrl));

            return Json(new { isValid = true });
        }

        public async Task<IActionResult> GetById(int id, PaginationFilter paginationFilter)
        {
            int userId = UserId;
            var contestSubmissions = await this.photoService
                .GetDetailedSubmissionsFromContestAsync(id, paginationFilter);

            foreach (var item in contestSubmissions.Model)
            {
                item.HasJuryGivenReview = await this.juryService.HasJuryAlreadyGivenReviewAsync(UserId, item.PhotoId);
            }

            var paginatedModel = new PaginatedModel<ContestSubmissionViewModel>()
            {
                Model = contestSubmissions.Model.Select(m => m.MapToContestSubmissionView())
                     .ToList(),
                RecordsPerPage = contestSubmissions.RecordsPerPage,
                TotalPages = contestSubmissions.TotalPages,
            };
            var contest = await this.contestService.GetByIdAsync(id);

            ViewData["Title"] = contest.Name;
            ViewData["Category"] = contest.ContestCategory;

            return View(paginatedModel);
        }

        public async Task<IActionResult> GetByIdUserSubmission(int id)
        {
            int userId = UserId;

            var submission = (await this.photoService
                .GetUserSubmissionForContestAsync(userId, id)
                ).MapToUserSubmissionViewModel();

            var contest = await this.contestService.GetByIdAsync(id);

            submission.ContestName = contest.Name;
            submission.ContestCatrogry = contest.ContestCategory;

            return View(submission);
        }

        public async Task<IActionResult> GiveReview(int contestId, int submitterId)
        {
            // Checks if user is jury and if phase is phase two
            if (!((await this.contestService.GetByIdAsync(contestId)).ActivePhase.Name == Constants.Phases.PhaseII &&
                await this.juryService.IsUserJuryForContest(contestId, UserId)))
            {
                return Unauthorized();
            }

            var submission = await this.photoService.GetUserSubmissionForContestAsync(submitterId, contestId);

            var giveReviewViewModel = new GiveReviewViewModel
            {
                PhotoUrl = submission.Url,
                Author = submission.SubmitterName,
                Description = submission.Description,
                Title = submission.Title,
                HasJuryGivenReview = await this.juryService.IsJuryGivenReviewForPhotoAsync(submission.Id, this.UserId)
            };

            if (giveReviewViewModel.HasJuryGivenReview)
            {
                var review = await this.juryService.GetReviewAsync(UserId, submission.Id);
                giveReviewViewModel.Review = new InputGiveReviewDto()
                {
                    Score = review.Score,
                    Comment = review.Comment,
                    Checkbox = review.IsDisqualified,
                };
            }
            else
            {
                giveReviewViewModel.Review = new InputGiveReviewDto()
                {
                    JuryId = this.UserId,
                    PhotoId = submission.Id,
                };
            }

            return View("~/Views/Dashboard/GiveReview.cshtml",
                giveReviewViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> GiveReview(GiveReviewViewModel model)
        {
            model.JuryId = UserId;

            if (!await juryService.IsContestInPhaseTwoAsync(model.Review.PhotoId))
            {
                ModelState
                   .AddModelError(string.Empty,
                   errorMessage: ErrorMessages.ReviewOutsidePhaseTwo);
            }

            if (!ModelState.IsValid)
            {
                return View("~/Views/Dashboard/GiveReview.cshtml", model);
            }

            if (await juryService.HasJuryAlreadyGivenReviewAsync(model.JuryId, model.PhotoId))
            {
                ModelState
                   .AddModelError(string.Empty,
                   errorMessage: ErrorMessages.ReviewAlreadyGiven);
            }

            var review = await this.juryService.GiveReviewAsync(model.MapToInputGiveReviewDto());
            model.HasJuryGivenReview = true;

            TempData["success"] = Constants.SuccessMessages.GivenReviewSuccess;

            return RedirectToAction(nameof(GetById), new { id = review.ContestId });
        }

        public async Task<IActionResult> DetailsReview(int contestId, int photoId, PaginationFilter paginationFilter)
        {
            //Checks if user is participant/jury and if contest is in phase - Finished
            if (!((await this.juryService.IsUserJuryForContest(contestId, UserId) ||
                await this.photoJunkieService.IsUserParticipant(contestId, this.UserId)) &&
                await this.contestService.IsContestInPhaseFinished(contestId)))
            {
                return Unauthorized();
            }

            var submissions = await this.photoService.GetDetailedSubmissionsForPhoto(contestId, photoId, paginationFilter);

            return View(submissions);
        }

        private async Task<PaginatedModel<DashboardViewModel>> GetContestViewModelByPhaseAsync(int categoryId, PaginationFilter paginationFilter, string phase)
        {
            var phaseOneContests = await this.contestService.GetAllForUserByPhaseAsync(UserId, paginationFilter, categoryId, phase);

            var contestByPhaseViewModel = new PaginatedModel<DashboardViewModel>()
            {
                Model = phaseOneContests.Model.Select(x => x.MapToViewDashboard()).ToList(),
                RecordsPerPage = phaseOneContests.RecordsPerPage,
                TotalPages = phaseOneContests.TotalPages,
            };

            foreach (var viewModel in contestByPhaseViewModel.Model)
            {
                viewModel.HasCurrentUserSybmittedPhoto = await this.photoJunkieService.HasCurrentUserSubmittedPhoto(UserId, viewModel.ContestId);
            }

            return contestByPhaseViewModel;
        }
    }
}
