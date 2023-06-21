using VolunteerWorkApi.Dtos.SavedFile;

namespace VolunteerWorkApi.Dtos.VolunteerProgramActivity
{
    public record VolunteerProgramActivityDto
    {
        public long Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public long VolunteerProgramId { get; set; }

        public ICollection<SavedFileDto> Photos { get; set; }
    }
}
