namespace VolunteerWorkApi.Entities
{
    public class Conversation : BaseEntity
    {
        public long? User1Id { get; set; }

        public long? User2Id { get; set; }

        public ApplicationUser User1 { get; set; }

        public ApplicationUser User2 { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
}
