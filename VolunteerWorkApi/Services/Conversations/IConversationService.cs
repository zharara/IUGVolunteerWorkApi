using VolunteerWorkApi.Dtos.Conversation;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Services.Conversations
{
    public interface IConversationService
    {
        IEnumerable<ConversationDto> GetAll(long currentUserId);

        IEnumerable<ConversationDto> GetList(
             long currentUserId,
             int? skipCount, int? maxResultCount,
             long? senderId, long? recieverId);

        ConversationDto? GetBetween(long user1Id, long user2Id);

        long? GetBetweenId(long user1Id, long user2Id);

        ConversationDto GetById(long id);

        Conversation? GetEntityById(long id);

        Task<ConversationDto> Create(
            CreateConversation createConversation, long currentUserId);

        Task<ConversationDto> Remove(long id);
    }
}
