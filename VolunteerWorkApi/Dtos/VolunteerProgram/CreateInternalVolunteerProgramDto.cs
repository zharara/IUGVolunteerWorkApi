using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Dtos.Category;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Dtos.VolunteerProgram
{
    public record CreateInternalVolunteerProgramDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public string Title { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public string Description { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public DateTime EndDate { get; set; }

        public long? CategoryId { get; set; }

        public CreateCategoryDto? Category { get; set; }

        public SaveTempFile? SaveTempFile { get; set; }
    }
}
