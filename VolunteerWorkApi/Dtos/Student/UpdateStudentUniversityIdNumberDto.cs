using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;

namespace VolunteerWorkApi.Dtos.Student
{
    public record UpdateStudentUniversityIdNumberDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long Id { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public int UniversityIdNumber { get; set; }
    }
}
