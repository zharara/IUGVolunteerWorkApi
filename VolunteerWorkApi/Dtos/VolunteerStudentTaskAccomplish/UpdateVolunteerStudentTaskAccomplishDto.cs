using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;

namespace VolunteerWorkApi.Dtos.VolunteerStudentTaskAccomplish
{
    public record UpdateVolunteerStudentTaskAccomplishDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long Id { get; set; }

        public bool? IsAccomplished { get; set; }

        public double? Rate { get; set; }
    }
}
