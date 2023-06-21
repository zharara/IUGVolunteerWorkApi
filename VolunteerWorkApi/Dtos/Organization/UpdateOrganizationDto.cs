using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Dtos.Organization
{
    public record UpdateOrganizationDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long Id { get; set; }

        public string? OrgNameLocal { get; set; }

        public string? OrgNameForeign { get; set; }

        public string? FieldOfWork { get; set; }

        public string? About { get; set; }

        public string? Vision { get; set; }

        public string? Mission { get; set; }

        public string? Address { get; set; }

        public SaveTempFile? SaveTempFile { get; set; }
    }
}
