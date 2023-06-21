using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Dtos.VolunteerProgramGalleryPhoto
{
    public record UpdateGalleryPhotoByStudentDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long Id { get; set; }

        public SaveTempFile? SaveTempFile { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long VolunteerStudentUploaderId { get; set; }
    }
}
