using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;

namespace VolunteerWorkApi.Dtos.Category
{
    public record CreateCategoryDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public string Name { get; set; }
    }
}
