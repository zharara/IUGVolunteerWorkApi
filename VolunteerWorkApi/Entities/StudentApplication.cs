using VolunteerWorkApi.Enums;

namespace VolunteerWorkApi.Entities
{
    public class StudentApplication : BaseEntity
    {
        public long StudentId { get; set; }

        public Student Student { get; set; }

        public long? VolunteerOpportunityId { get; set; }

        public VolunteerOpportunity VolunteerOpportunity { get; set; }

        public ApplicationStatus StatusForOrganization { get; set; }

        public ApplicationStatus StatusForManagement { get; set; }

        public string? TextInformation { get; set; }

        public SavedFile? SubmittedFile { get; set; }

        public string? OrganizationRejectionReason { get; set; }

        public string? ManagementRejectionReason { get; set; }
    }
}
