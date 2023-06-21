using VolunteerWorkApi.Dtos.Student;
using VolunteerWorkApi.Dtos.VolunteerProgram;

namespace VolunteerWorkApi.Dtos.VolunteerStudent
{
    public record VolunteerStudentDto
    {
        public long Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public long StudentId { get; set; }

        public StudentDto Student { get; set; }

        public long VolunteerProgramId { get; set; }

        public VolunteerProgramDto VolunteerProgram { get; set; }

        public double? OrgAssessmentGrade { get; set; }

        public string? OrgAssessmentGradeNotes { get; set; }

        public double? FinalGrade { get; set; }

        public string? FinalGradeNotes { get; set; }
    }
}
