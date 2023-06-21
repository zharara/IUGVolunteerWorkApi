using VolunteerWorkApi.Dtos.Category;

namespace VolunteerWorkApi.Dtos.Interest
{
    public record InterestDto
    {
        public long Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Name { get; set; }

        public CategoryDto Category { get; set; }
    }
}
