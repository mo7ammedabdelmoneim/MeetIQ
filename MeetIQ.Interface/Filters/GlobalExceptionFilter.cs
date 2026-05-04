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
            var errorCode = "500";

            switch (exception)
            {
                case BadRequestException:
                    errorCode = "400";
                    message = exception.Message;
                    break;

                case NotFoundException:
                    errorCode = "404";
                    message = exception.Message;
                    break;

                case UnauthorizedException:
                    errorCode = "401";
                    message = exception.Message;
                    break;
            }

            var tempData = _tempDataFactory.GetTempData(context.HttpContext);
            tempData["ErrorMessage"] = message;
            tempData["ErrorCode"] = errorCode;

            context.Result = new RedirectToActionResult("Error", "Home", null);
            context.ExceptionHandled = true;
        }
    }
}