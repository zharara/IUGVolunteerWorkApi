using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Dtos.VolunteerProgramActivity
{
    public record CreateVolunteerProgramActivityDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public string Title { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public string Description { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long VolunteerProgramId { get; set; }

        public ICollection<SaveTempFile>? SaveTempFiles { get; set; }
    }
}
