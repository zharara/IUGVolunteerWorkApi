using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Dtos.VolunteerProgramGalleryPhoto
{
    public record UpdateGalleryPhotoDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long Id { get; set; }

        public SaveTempFile? SaveTempFile { get; set; }

        public bool? IsApproved { get; set; }
    }
}
