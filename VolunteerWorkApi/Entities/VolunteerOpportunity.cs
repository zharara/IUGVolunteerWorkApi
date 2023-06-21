
namespace VolunteerWorkApi.Entities
{
    public class VolunteerOpportunity : BaseEntity
    {
        public long OrganizationId { get; set; }

        public Organization Organization { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string NatureOfWorkOrActivities { get; set; }

        public long CategoryId { get; set; }

        public Category Category { get; set; }

        public SavedFile? Logo { get; set; }

        public DateTime ActualProgramStartDate { get; set; }

        public DateTime ActualProgramEndDate { get; set; }

        public DateTime? AnnouncementEndDate { get; set; }

        public DateTime ReceiveApplicationsEndDate { get; set; }

        public int RequiredVolunteerStudentsNumber { get; set; }

        public ICollection<Interest> VolunteerInterests { get; set; }

        public ICollection<Skill> VolunteerSkills { get; set; }

        public string? ApplicantQualifications { get; set; }

        public bool IsRequirementNeededAsText { get; set; }

        public bool IsRequirementNeededAsFile { get; set; }

        public string? RequirementFileDescription { get; set; }

        public double? RequirementFileMaxAllowedSize { get; set; }

        public string? RequirementFileAllowedTypes { get; set; }
    }
}
