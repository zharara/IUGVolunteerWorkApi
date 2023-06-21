
namespace VolunteerWorkApi.Models
{
    public record CreateConversation
    {
        public long User1Id { get; set; }

        public long User2Id { get; set; }
    }
}
