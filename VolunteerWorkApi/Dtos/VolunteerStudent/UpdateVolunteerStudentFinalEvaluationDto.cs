using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;

namespace VolunteerWorkApi.Dtos.VolunteerStudent
{
    public record UpdateVolunteerStudentFinalEvaluationDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long Id { get; set; }

        public double? FinalGrade { get; set; }

        public string? FinalGradeNotes { get; set; }
    }
}
