using FullFraim.Models.ViewModels.ContactUs;
using FullFraim.Models.ViewModels.Home;
using FullFraim.Services.PhotoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.AllConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Mailing;
using Utilities.Mapper;

namespace FullFraim.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : BaseMvcController
    {
        private readonly ILogger<HomeController> logger;
        private readonly IPhotoService photoService;
        private readonly IMemoryCache cache;
        private readonly IEmailSender emailSender;
        private readonly IConfiguration configuration;

        public HomeController(ILogger<HomeController> logger,
            IPhotoService photoService,
            IMemoryCache cache,
            IEmailSender emailSender,
            IConfiguration configuration)
        {
            this.logger = logger;
            this.photoService = photoService;
            this.cache = cache;
            this.emailSender = emailSender;
            this.configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            if (!cache.TryGetValue<ICollection<HomeIndexViewModel>>("photos", out var photos))
            {
                photos = (await this.photoService.GetTopRecentPhotosAsync())
                    .Select(p => p.MapToHomeViewModel()).ToList();
                // If no photos are submitted yet
                if (photos.Count == 0)
                {
                    for (int i = 0; i < TopPhotos.AuthorNames.Count; i++)
                    {
                        photos.Add(new HomeIndexViewModel
                        {
                            PhotoUrl = TopPhotos.Photos[i],
                            SubmitterName = TopPhotos.AuthorNames[i],
                            Title = TopPhotos.Titles[i],
                        });
                    }
                }
                cache.Set("photos", photos, TimeSpan.FromDays(1));
            }

            return View(photos);
        }

        public IActionResult ContactUs()
        {
            return new PartialViewResult()
            {
                ViewName = "~/Views/Shared/Partials/_ContactFormPartial.cshtml",
            };
        }

        [HttpPost]
        public async Task<IActionResult> ContactUs(ContactUsInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return new PartialViewResult()
                {
                    ViewName = "~/Views/Shared/Partials/_ContactFormPartial.cshtml",
                };
            }

            await this.emailSender.SendEmailAsync(Sender: this.configuration["SendGrid:SenderEmail"],
                        SenderName: Constants.Email.SenderName,
                        To: this.configuration["SendGrid:SenderEmail"],
                        Subject: inputModel.Subject,
                        HtmlContent: $"<b>{inputModel.Email}<b/> contacted us with message:\n {inputModel.Message}");

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Exc()
        {
            return StatusCode(500);
        }

        [HttpGet]
        public IActionResult Un()
        {
            return StatusCode(403);
        }
    }
}
