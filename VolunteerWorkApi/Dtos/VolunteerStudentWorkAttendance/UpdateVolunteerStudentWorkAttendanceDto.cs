using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;

namespace VolunteerWorkApi.Dtos.VolunteerStudentWorkAttendance
{
    public record UpdateVolunteerStudentWorkAttendanceDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long Id { get; set; }

        public bool? IsAttended { get; set; }
    }
}
