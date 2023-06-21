namespace VolunteerWorkApi.Entities
{
    public class Announcement : BaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public SavedFile? Image { get; set; }

        public bool IsManagementAnnouncement { get; set; } = false;

        public bool IsOrganizationAnnouncement { get; set; } = false;

        public long? OrganizationId { get; set; }

        public Organization? Organization { get; set; }

        public long? VolunteerProgramId { get; set; }

        public VolunteerProgram? VolunteerProgram { get; set; }
    }
}
