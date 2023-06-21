using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;

namespace VolunteerWorkApi.Dtos.VolunteerProgramWorkDay
{
    public record CreateVolunteerProgramWorkDayDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public string Name { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long VolunteerProgramId { get; set; }
    }
}
