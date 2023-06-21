using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Enums;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Dtos.StudentApplication
{
    public record CreateStudentApplicationDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long StudentId { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long VolunteerOpportunityId { get; set; }

        public string? TextInformation { get; set; }

        public SaveTempFile? SaveTempFile { get; set; }
    }
}
