using VolunteerWorkApi.Dtos.Category;
using VolunteerWorkApi.Dtos.Organization;
using VolunteerWorkApi.Dtos.SavedFile;

namespace VolunteerWorkApi.Dtos.VolunteerProgram
{
    public record VolunteerProgramDto
    {
        public long Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsInternalProgram { get; set; }

        public long? OrganizationId { get; set; }

        public OrganizationDto? Organization { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public long CategoryId { get; set; }

        public CategoryDto Category { get; set; }

        public SavedFileDto? Logo { get; set; }
    }
}
