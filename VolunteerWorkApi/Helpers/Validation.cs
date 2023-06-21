using System.ComponentModel.DataAnnotations;

namespace VolunteerWorkApi.Helpers
{
    public class Validation
    {
        public static bool IsValidEmailAddress(string email)
        {
            if (!string.IsNullOrEmpty(email) && new EmailAddressAttribute().IsValid(email))
                return true;
            else
                return false;
        }
    }
}
