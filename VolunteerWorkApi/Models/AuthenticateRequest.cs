using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;

namespace VolunteerWorkApi.Models
{
    public record AuthenticateRequest
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public string Password { get; set; }

        public string? FCMToken { get; set; }
    }
}
