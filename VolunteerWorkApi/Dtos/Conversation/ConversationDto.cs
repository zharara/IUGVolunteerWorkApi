using VolunteerWorkApi.Dtos.ApplicationUser;

namespace VolunteerWorkApi.Dtos.Conversation
{
    public record ConversationDto
    {
        public long Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public long User1Id { get; set; }

        public long User2Id { get; set; }

        public ApplicationUserDto User1 { get; set; }

        public ApplicationUserDto User2 { get; set; }
    }
}
