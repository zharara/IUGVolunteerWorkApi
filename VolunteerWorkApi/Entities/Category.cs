namespace VolunteerWorkApi.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<VolunteerOpportunity> VolunteerOpportunities { get; set; }

        public ICollection<VolunteerProgram> VolunteerPrograms { get; set; }

        public ICollection<Skill> Skills { get; set; }

        public ICollection<Interest> Interests { get; set; }
    }
}
