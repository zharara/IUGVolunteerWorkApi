using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Dtos.Category;

namespace VolunteerWorkApi.Dtos.Skill
{
    public record CreateSkillDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public string Name { get; set; }

        public long? CategoryId { get; set; }

        public CreateCategoryDto? Category { get; set; }
    }
}
