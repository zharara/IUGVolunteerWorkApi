using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Dtos.Interest;

namespace VolunteerWorkApi.Models
{
    public record UpdateStudentInterests
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long StudentId { get; set; }

        public ICollection<ExistingOrCreateNewInterestDto> Interests { get; set; }
    }
}
