using VolunteerWorkApi.Dtos.ApplicationUser;
using VolunteerWorkApi.Dtos.Conversation;

namespace VolunteerWorkApi.Dtos.Message
{
    public record MessageDto
    {
        public long Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Content { get; set; }

        public long? SenderId { get; set; }

        public long? ReceiverId { get; set; }

        public ApplicationUserDto Sender { get; set; }

        public ApplicationUserDto Receiver { get; set; }

        public long ConversationId { get; set; }

        public ConversationDto Conversation { get; set; }
    }
}
