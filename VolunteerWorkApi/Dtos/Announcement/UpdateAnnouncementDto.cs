using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Dtos.Announcement
{
    public record UpdateAnnouncementDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public SaveTempFile? SaveTempFile { get; set; }
    }
}
