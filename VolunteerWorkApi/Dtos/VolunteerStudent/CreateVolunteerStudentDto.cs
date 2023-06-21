using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;

namespace VolunteerWorkApi.Dtos.VolunteerStudent
{
    public record CreateVolunteerStudentDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long StudentId { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long VolunteerProgramId { get; set; }
    }
}
