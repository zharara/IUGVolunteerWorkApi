using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;

namespace VolunteerWorkApi.Dtos.VolunteerStudentActivityAttendance
{
    public record CreateVolunteerStudentActivityAttendanceDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public bool IsAttended { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long VolunteerStudentId { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long VolunteerProgramActivityId { get; set; }
    }
}
