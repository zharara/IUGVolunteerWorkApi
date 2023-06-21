using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;

namespace VolunteerWorkApi.Dtos.VolunteerStudentActivityAttendance
{
    public record UpdateVolunteerStudentActivityAttendanceDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long Id { get; set; }

        public bool? IsAttended { get; set; }
    }
}
