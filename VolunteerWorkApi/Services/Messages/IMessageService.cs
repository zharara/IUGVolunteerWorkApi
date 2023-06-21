using VolunteerWorkApi.Dtos.Message;

namespace VolunteerWorkApi.Services.Messages
{
    public interface IMessageService
    {
        IEnumerable<MessageDto> GetList(
             long currentUserId,
             int? skipCount, int? maxResultCount,
             long? conversationId);

        MessageDto GetById(long id);

        Task<MessageDto> Create(
            CreateMessageDto createEntityDto,
            long currentUserId, long? conversationId);

        Task<MessageDto> Update(
            UpdateMessageDto updateEntityDto, long currentUserId);

        Task<MessageDto> Remove(long id);
    }
}
