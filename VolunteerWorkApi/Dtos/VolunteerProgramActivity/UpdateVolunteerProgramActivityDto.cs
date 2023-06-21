using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Dtos.VolunteerProgramActivity
{
    public record UpdateVolunteerProgramActivityDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public ICollection<SaveTempFile>? SaveTempFiles { get; set; }
    }
}
