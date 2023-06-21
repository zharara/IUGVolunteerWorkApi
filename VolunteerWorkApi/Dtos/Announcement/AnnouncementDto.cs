using VolunteerWorkApi.Dtos.SavedFile;

namespace VolunteerWorkApi.Dtos.Announcement
{
    public record AnnouncementDto
    {
        public long Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public SavedFileDto? Image { get; set; }

        public bool IsManagementAnnouncement { get; set; }

        public bool IsOrganizationAnnouncement { get; set; }

        public long? OrganizationId { get; set; }

        public long? VolunteerProgramId { get; set; }
    }
}
