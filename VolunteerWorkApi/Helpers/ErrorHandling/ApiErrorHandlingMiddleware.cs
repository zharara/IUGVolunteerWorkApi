using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Authentication;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Helpers.ErrorHandling
{
    public class ApiErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            try
            {
                await next(httpContext);

                if (httpContext.Response.HasStarted)
                {
                    return;
                }

                if (httpContext.Response.StatusCode == 401)
                {
                    throw new AuthenticationException();
                }
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {

            int statusCode = (int)HttpStatusCode.InternalServerError;
            string errorTitle = ErrorMessages.ServerError;
            string errorMessage = ErrorMessages.InternalServerErrorOccurred;
            string? errorDetails = null;

            if (ex is ApiResponseException apiResponseException)
            {
                statusCode = (int)apiResponseException.StatusCode;
                errorTitle = apiResponseException.ErrorTitle;
                errorMessage = apiResponseException.ErrorMessage;
                errorDetails = apiResponseException.ErrorDetails;
            }
            else if (ex is ApiNotFoundException)
            {
                statusCode = (int)HttpStatusCode.NotFound;
                errorTitle = ErrorMessages.NotFound;
                errorMessage = ErrorMessages.DataNotFound;
            }
            else if (ex is ApiDataException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                errorTitle = ErrorMessages.DataError;
                errorMessage = ErrorMessages.DataHandlingError;
            }
            else if (ex is AuthenticationException)
            {
                statusCode = (int)HttpStatusCode.Unauthorized;
                errorTitle = ErrorMessages.AccessDenied;
                errorMessage = ErrorMessages.NoPermissionsToAccess;
            }
            else if (ex is ValidationException validationException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                errorTitle = ErrorMessages.InputError;
                errorMessage = ErrorMessages.RequiredFieldsNotInputted;
                errorDetails = validationException.ValidationAttribute?.ErrorMessage;
            }

            context.Response.StatusCode = statusCode;
            var errorResponse = new ApiError
            {
                ErrorTitle = errorTitle,
                ErrorMessage = errorMessage,
                ErrorDetails = errorDetails,
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        }

    }
}