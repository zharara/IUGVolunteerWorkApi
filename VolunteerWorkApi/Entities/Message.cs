namespace VolunteerWorkApi.Entities
{
    public class Message : BaseEntity
    {
        public string Content { get; set; }

        public long? SenderId { get; set; }

        public long? ReceiverId { get; set; }

        public ApplicationUser Sender { get; set; }

        public ApplicationUser Receiver { get; set; }

        public long ConversationId { get; set; }

        public Conversation Conversation { get; set; }
    }
}
