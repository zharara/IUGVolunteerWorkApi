using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Dtos.StudentApplication
{
    public record UpdateStudentApplicationDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long Id { get; set; }

        public string? TextInformation { get; set; }

        public SaveTempFile? SaveTempFile { get; set; }
    }
}
