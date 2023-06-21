using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Dtos.ManagementEmployee
{
    public record UpdateManagementEmployeeDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long Id { get; set; }

        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }

        public SaveTempFile? SaveTempFile { get; set; }
    }
}
