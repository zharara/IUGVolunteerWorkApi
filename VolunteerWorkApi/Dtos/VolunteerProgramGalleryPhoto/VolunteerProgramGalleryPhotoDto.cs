using VolunteerWorkApi.Dtos.SavedFile;

namespace VolunteerWorkApi.Dtos.VolunteerProgramGalleryPhoto
{
    public record VolunteerProgramGalleryPhotoDto
    {
        public long Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsApproved { get; set; }

        public SavedFileDto Photo { get; set; }

        public long VolunteerProgramId { get; set; }

        public long? VolunteerStudentUploaderId { get; set; }
    }
}
