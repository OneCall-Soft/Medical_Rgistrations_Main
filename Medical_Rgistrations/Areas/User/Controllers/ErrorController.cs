using Microsoft.AspNetCore.Mvc;

namespace Medical_Rgistrations.Controllers
{
    using Medical_Rgistrations.ControllerBase;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Mvc;

    namespace EmptyNetCore.Controllers
    {
        [Area("Administrator")]
        public class ErrorController : BasePageController
        {
            [Route("Error/{statusCode}")]
            public IActionResult HttpStatusCodeHandler(int statusCode)
            {

                var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
                switch (statusCode)
                {
                    case 404:
                        ViewBag.ErrorMessage = "Sorry, the resource you request could not be found";
                        ViewBag.Path = statusCodeResult.OriginalPath;
                        ViewBag.QS = statusCodeResult.OriginalQueryString;
                        break;

                    default:
                        break;
                }

                return View("NotFound");
            }

            [Route("Error")]
            [AllowAnonymous]
            public IActionResult Error()
            {

                var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerFeature>();

                if (exceptionDetails != null)
                {
                    ViewBag.ExceptionPath = exceptionDetails.Path;
                    ViewBag.ExceptionMessage = exceptionDetails.Error.Message;
                    ViewBag.StackTrace = exceptionDetails.Error.StackTrace;
                }

                return View("Error");
            }
        }



    }
}
