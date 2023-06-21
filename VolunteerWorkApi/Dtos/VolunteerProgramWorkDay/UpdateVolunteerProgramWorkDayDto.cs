using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;

namespace VolunteerWorkApi.Dtos.VolunteerProgramWorkDay
{
    public record UpdateVolunteerProgramWorkDayDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long Id { get; set; }

        public string? Name { get; set; }

        public DateTime? Date { get; set; }
    }
}
