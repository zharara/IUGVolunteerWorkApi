using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Dtos.ManagementEmployee
{
    public record CreateManagementEmployeeDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public string UserName { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public string Password { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public string FirstName { get; set; }

        public string? MiddleName { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public string LastName { get; set; }

        public SaveTempFile? SaveTempFile { get; set; }

        public string? FCMToken { get; set; }
    }
}
