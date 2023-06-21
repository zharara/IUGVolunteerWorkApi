using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;

namespace VolunteerWorkApi.Dtos.VolunteerStudent
{
    public record UpdateVolunteerStudentOrgAssessmentDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long Id { get; set; }

        public double? OrgAssessmentGrade { get; set; }

        public string? OrgAssessmentGradeNotes { get; set; }

    }
}
