
using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Dtos.Category;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Dtos.VolunteerProgram
{
    public record UpdateVolunteerProgramDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public long? CategoryId { get; set; }

        public CreateCategoryDto? Category { get; set; }

        public SaveTempFile? SaveTempFile { get; set; }
    }
}
