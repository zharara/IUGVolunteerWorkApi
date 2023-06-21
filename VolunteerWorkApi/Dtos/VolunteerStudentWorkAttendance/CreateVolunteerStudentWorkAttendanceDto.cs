using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;

namespace VolunteerWorkApi.Dtos.VolunteerStudentWorkAttendance
{
    public record CreateVolunteerStudentWorkAttendanceDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public bool IsAttended { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long VolunteerStudentId { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long VolunteerProgramWorkDayId { get; set; }
    }
}
