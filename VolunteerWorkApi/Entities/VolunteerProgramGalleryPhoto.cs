namespace VolunteerWorkApi.Entities
{
    public class VolunteerProgramGalleryPhoto : BaseEntity
    {
        public bool IsApproved { get; set; }

        public SavedFile Photo { get; set; }

        public long VolunteerProgramId { get; set; }

        public VolunteerProgram VolunteerProgram { get; set; }

        public long? VolunteerStudentUploaderId { get; set; }

        public VolunteerStudent? VolunteerStudentUploader { get; set; }
    }
}
