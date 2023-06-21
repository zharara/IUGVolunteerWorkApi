using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Primitives;
using System.Net;
using System.Text;
using VolunteerWorkApi.Constants;

namespace VolunteerWorkApi.Helpers.ErrorHandling
{
    public static class ConvertErrors
    {
        public static ApiResponseException ConvertIdentityResultErrors(
            IEnumerable<IdentityError> errors)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var error in errors)
            {
                sb.Append($"الرمز: {error.Code} \n{error.Description}\n");
            }

            return new ApiResponseException(
                HttpStatusCode.Unauthorized,
                ErrorMessages.AuthError,
                ErrorMessages.ErrorWhileTryingToCreateAccount,
                sb.ToString());
        }
    }
}
