using VolunteerWorkApi.Dtos.SavedFile;
using VolunteerWorkApi.Dtos.Student;
using VolunteerWorkApi.Dtos.VolunteerOpportunity;
using VolunteerWorkApi.Enums;

namespace VolunteerWorkApi.Dtos.StudentApplication
{
    public record StudentApplicationDto
    {
        public long Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public long StudentId { get; set; }

        public StudentDto Student { get; set; }

        public long? VolunteerOpportunityId { get; set; }

        public VolunteerOpportunityDto VolunteerOpportunity { get; set; }

        public ApplicationStatus StatusForOrganization { get; set; }

        public ApplicationStatus StatusForManagement { get; set; }

        public string? TextInformation { get; set; }

        public SavedFileDto? SubmittedFile { get; set; }

        public string? OrganizationRejectionReason { get; set; }

        public string? ManagementRejectionReason { get; set; }
    }
}
