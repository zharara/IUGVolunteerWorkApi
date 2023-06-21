using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;

namespace VolunteerWorkApi.Dtos.VolunteerProgramTask
{
    public record CreateVolunteerProgramTaskDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public string Title { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public string Description { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long VolunteerProgramId { get; set; }
    }
}
