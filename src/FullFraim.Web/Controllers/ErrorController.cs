using FullFraim.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.AllConstants;
using System.Diagnostics;

namespace FullFraim.Web.Controllers
{
    public class ErrorController : Controller
    {
        public ErrorController()
        {

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("/Error/{statusCode}")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statusCode)
        {
            switch (statusCode)
            {
                case 403:
                    return View("Unauthorized", new ErrorViewModel { Message = ClientErrorMessages.Unauthorized });
                case 404:
                    return View("NotFound", new ErrorViewModel { Message = ClientErrorMessages.NotFound });
            }

            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ??
                HttpContext.TraceIdentifier,
                Message = ClientErrorMessages.ServerError
            });
        }
    }
}
