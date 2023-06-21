namespace VolunteerWorkApi.Entities
{
    public class VolunteerStudent : BaseEntity
    {
        public long StudentId { get; set; }

        public Student Student { get; set; }

        public long VolunteerProgramId { get; set; }

        public VolunteerProgram VolunteerProgram { get; set; }

        public double? OrgAssessmentGrade { get; set; }

        public string? OrgAssessmentGradeNotes { get; set; }

        public double? FinalGrade { get; set; }

        public string? FinalGradeNotes { get; set; }
    }
}
