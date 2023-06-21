using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VolunteerWorkApi.Models;
using VolunteerWorkApi.Constants;

namespace VolunteerWorkApi.Helpers.ErrorHandling
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order => int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                if (context.Exception is ApiResponseException httpResponseException)
                {
                    context.Result = new ObjectResult(
                        new ApiError
                        {
                            ErrorTitle = httpResponseException.ErrorTitle,
                            ErrorMessage = httpResponseException.ErrorMessage,
                            ErrorDetails = httpResponseException.ErrorDetails,
                        })
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                    };
                }
                else if (context.Exception is ApiNotFoundException)
                {
                    context.Result = new ObjectResult(
                       new ApiError
                       {
                           ErrorTitle = ErrorMessages.NotFound,
                           ErrorMessage = ErrorMessages.DataNotFound,
                       })
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                    };
                }
                else if (context.Exception is ApiDataException)
                {
                    context.Result = new ObjectResult(
                       new ApiError
                       {
                           ErrorTitle = ErrorMessages.DataError,
                           ErrorMessage = ErrorMessages.DataHandlingError,
                       })
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                    };
                }
                else
                {
                    context.Result = new ObjectResult(
                       new ApiError
                       {
                           ErrorTitle = ErrorMessages.ServerError,
                           ErrorMessage = ErrorMessages.InternalServerErrorOccurred,
                       })
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError,
                    };
                }

                context.ExceptionHandled = true;
            }
            else if (context.HttpContext.User == null || !(context.HttpContext.User.Identity?.IsAuthenticated ?? false))
            {
                context.Result = new ObjectResult(
                       new ApiError
                       {
                           ErrorTitle = ErrorMessages.DataError,
                           ErrorMessage = ErrorMessages.DataHandlingError,
                       })
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                };

                
            }
            else if (!context.ModelState.IsValid)
            {
                context.Result = new ObjectResult(
                       new ApiError
                       {
                           ErrorTitle = ErrorMessages.DataError,
                           ErrorMessage = ErrorMessages.DataHandlingError,
                       })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                };
            }
        }
    }
}
