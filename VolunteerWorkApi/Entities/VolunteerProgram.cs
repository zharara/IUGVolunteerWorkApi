
namespace VolunteerWorkApi.Entities
{
    public class VolunteerProgram : BaseEntity
    {
        public bool IsInternalProgram { get; set; } = false;

        public long? OrganizationId { get; set; }

        public Organization? Organization { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public long CategoryId { get; set; }

        public Category Category { get; set; }

        public SavedFile? Logo { get; set; }

        public ICollection<VolunteerStudent> VolunteerStudents { get; set; }

        public ICollection<VolunteerProgramTask> VolunteerProgramTasks { get; set; }

        public ICollection<VolunteerProgramActivity> VolunteerProgramActivities { get; set; }

        public ICollection<VolunteerProgramWorkDay> VolunteerWorkDays { get; set; }

        public ICollection<VolunteerProgramGalleryPhoto> VolunteerProgramGalleryPhotos { get; set; }
    }
}
