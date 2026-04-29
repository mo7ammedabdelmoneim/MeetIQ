using MeetIQ.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace MeetIQ.Interface.Filters
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ITempDataDictionaryFactory _tempDataFactory;

        public GlobalExceptionFilter(ITempDataDictionaryFactory tempDataFactory)
        {
            _tempDataFactory = tempDataFactory;
        }

        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var message = "Something went wrong. Please try again.";
            var action = "Index";
            var controller = "Home";

            switch (exception)
            {
                case BadRequestException:
                    message = exception.Message;
                    // Stay on same page so the user can correct their input
                    action = context.RouteData.Values["action"]?.ToString() ?? "Index";
                    controller = context.RouteData.Values["controller"]?.ToString() ?? "Home";
                    break;
                    
                case NotFoundException:
                    message = exception.Message;
                    action = "NotFound";
                    controller = "Home";
                    break;

                case UnauthorizedException:
                    message = exception.Message;
                    action = "Login";
                    controller = "Auth";
                    break;
            }

            var tempData = _tempDataFactory.GetTempData(context.HttpContext);
            tempData["ErrorMessage"] = message;

            context.Result = new RedirectToActionResult(action, controller, null);
            context.ExceptionHandled = true;
        }
    }
}