using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Dtos.Category;

namespace VolunteerWorkApi.Dtos.Interest
{
    public record UpdateInterestDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long Id { get; set; }

        public string? Name { get; set; }

        public long? CategoryId { get; set; }

        public CreateCategoryDto? Category { get; set; }
    }
}
