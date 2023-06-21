using VolunteerWorkApi.Dtos.Category;

namespace VolunteerWorkApi.Dtos.Skill
{
    public record SkillDto
    {
        public long Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Name { get; set; }

        public CategoryDto Category { get; set; }
    }
}
