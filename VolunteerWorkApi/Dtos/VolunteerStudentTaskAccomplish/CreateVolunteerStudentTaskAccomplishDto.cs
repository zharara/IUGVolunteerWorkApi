using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;

namespace VolunteerWorkApi.Dtos.VolunteerStudentTaskAccomplish
{
    public record CreateVolunteerStudentTaskAccomplishDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public bool IsAccomplished { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public double Rate { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long VolunteerStudentId { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long VolunteerProgramTaskId { get; set; }
    }
}
