using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Dtos.VolunteerProgramGalleryPhoto
{
    public record CreateGalleryPhotoByStudentDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public SaveTempFile SaveTempFile { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long VolunteerProgramId { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long VolunteerStudentUploaderId { get; set; }
    }
}
