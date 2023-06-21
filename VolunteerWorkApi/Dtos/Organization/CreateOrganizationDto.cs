using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Dtos.Organization
{
    public record CreateOrganizationDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public string UserName { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public string Password { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public string OrgNameLocal { get; set; }

        public string OrgNameForeign { get; set; } = "";

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public string FieldOfWork { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public string About { get; set; }

        public string? Vision { get; set; }

        public string? Mission { get; set; }

        public string? Address { get; set; }

        public SaveTempFile? SaveTempFile { get; set; }

        public string? FCMToken { get; set; }
    }
}
