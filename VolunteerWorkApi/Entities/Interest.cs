namespace VolunteerWorkApi.Entities
{
    public class Interest : BaseEntity
    {
        public string Name { get; set; }

        public long CategoryId { get; set; }

        public Category Category { get; set; }

        public ICollection<Student> Students { get; set; }
    }
}
