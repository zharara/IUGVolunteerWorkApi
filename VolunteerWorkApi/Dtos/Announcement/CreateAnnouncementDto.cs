using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Dtos.Announcement
{
    public record CreateAnnouncementDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public string Title { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public string Description { get; set; }

        public SaveTempFile? SaveTempFile { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public bool IsManagementAnnouncement { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public bool IsOrganizationAnnouncement { get; set; }

        public long? OrganizationId { get; set; }

        public long? VolunteerProgramId { get; set; }
    }
}
