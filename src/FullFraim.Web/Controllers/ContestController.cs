using FullFraim.Models.Contest.ViewModels;
using FullFraim.Models.Dto_s.Pagination;
using FullFraim.Services.ContestCatgeoryServices;
using FullFraim.Services.ContestServices;
using FullFraim.Services.ContestTypeServices;
using FullFraim.Services.PhaseServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.AllConstants;
using System;
using System.Linq;
using System.Threading.Tasks;
using Utilities.CloudinaryUtils;
using Utilities.Mapper;

namespace FullFraim.Web.Controllers
{
    [Authorize]
    [Controller]
    public class ContestController : BaseMvcController
    {
        private readonly IContestService contestService;
        private readonly IContestCategoryService contestCategoryService;
        private readonly IPhaseService phaseService;
        private readonly IContestTypeService contestTypeService;
        private readonly ICloudinaryUtils cloudinaryService;

        public ContestController(IContestService contestService,
            IContestCategoryService contestCategoryService,
            IPhaseService phaseService,
            IContestTypeService contestTypeService,
            ICloudinaryUtils cloudinaryService)
        {
            this.contestService = contestService;
            this.contestCategoryService = contestCategoryService;
            this.phaseService = phaseService;
            this.contestTypeService = contestTypeService;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<IActionResult> Create()
        {
            await SeedDropdownsForContest();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateContestViewModel model)
        {
            foreach (var jury in model.Juries)
            {
                if (model.Participants.Contains(jury))
                {
                    ModelState.AddModelError(string.Empty, ErrorMessages.JuryCannotBeParticipant);
                    break;
                }
            }

            if (!await this.contestService.IsNameUniqueAsync(model.Name))
            {
                ModelState.AddModelError(string.Empty, ErrorMessages.NameMustBeUnique);
            }

            if (model.Cover_Url != null && model.Cover != null)
            {
                ModelState
                    .AddModelError(string.Empty, ErrorMessages.CannotSendBothUrlAndImage);
            }

            if (model.Cover == null && model.Cover_Url == null)
            {
                ModelState
                    .AddModelError(string.Empty, ErrorMessages.ContestCoverRequired);
            }

            if (!ModelState.IsValid)
            {
                await SeedDropdownsForContest();

                return View(model);
            }

            var invitationalContestTypeId =
                (await this.contestTypeService.GetAllAsync())
                    .FirstOrDefault(ct => ct.Name == Constants.ContestType.Invitational).Id;

            if (model.ContestTypeId == invitationalContestTypeId &&
                    model.Juries == null &&
                    model.Participants == null)
            {
                throw new Exception();
            }

            if (model.Cover != null && model.Cover_Url == null)
            {
                model.Cover_Url = this.cloudinaryService.UploadImage(model.Cover);
            }

            var contest = await this.contestService.CreateAsync(model.MapToDto());

            TempData["success"] = Constants.SuccessMessages.CreateContestSuccess;

            return RedirectToAction(nameof(DashboardController.Index),
                nameof(DashboardController).Replace("Controller", string.Empty));
        }

        [HttpGet]
        public async Task<IActionResult> ChooseCovers()
        {
            var result = await this.contestService.GetContestCoversAsync(new PaginationFilter());

            ViewBag.Covers = result;

            return View();
        }

        [NonAction]
        private async Task SeedDropdownsForContest()
        {
            ViewBag.Categories = await this.contestCategoryService
                    .GetAllAsync();

            ViewBag.Phases = await this.phaseService
                .GetAllAsync();

            ViewBag.ContestTypes = await this.contestTypeService
                .GetAllAsync();

            ViewBag.Jury = (await this.contestService.GetPotentialJuryForInvitationAsync()).MapToDropDownView();
            ViewBag.Participants = (await this.contestService.GetParticipantsForInvitationAsync()).MapToDropDownView();
        }
    }
}
