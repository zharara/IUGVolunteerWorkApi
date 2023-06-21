using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Data;
using VolunteerWorkApi.Dtos.Conversation;
using VolunteerWorkApi.Dtos.Message;
using VolunteerWorkApi.Extensions;
using VolunteerWorkApi.Helpers.ErrorHandling;
using VolunteerWorkApi.Models;
using VolunteerWorkApi.Services.Conversations;

namespace VolunteerWorkApi.Services.Messages
{
    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IConversationService _conversationService;

        public MessageService(ApplicationDbContext dbContext,
            IMapper mapper, IConversationService conversationService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _conversationService = conversationService;
        }

        public IEnumerable<MessageDto> GetList(
           long currentUserId, int? skipCount,
           int? maxResultCount, long? conversationId)
        {
            return _dbContext.Messages
                 .Include(x => x.Sender).ThenInclude(x => x.ProfilePicture)
                 .Include(x => x.Receiver).ThenInclude(x => x.ProfilePicture)
                 .WhereIf(conversationId == null,
                    x => x.SenderId == currentUserId
                    || x.ReceiverId == currentUserId)
                 .WhereIf(conversationId != null,
                    x => x.ConversationId == conversationId)
                 .Skip(skipCount ?? 0)
                 .Take(maxResultCount ?? ApiConstants.MaxResultCount)
                 .Select(x => _mapper.Map<MessageDto>(x))
                 .ToList();
        }

        public MessageDto GetById(long id)
        {
            var data = _dbContext.Messages.Find(id);

            if (data == null)
            {
                throw new ApiNotFoundException();
            }

            return _mapper.Map<MessageDto>(data);
        }

        public async Task<MessageDto> Create(
            CreateMessageDto createEntityDto,
            long currentUserId, long? conversationId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                conversationId ??= await GetConversationId(
                        createEntityDto.SenderId,
                        createEntityDto.ReceiverId,
                        currentUserId);

                if (conversationId == null)
                {
                    throw new ApiNotFoundException();
                }

                var entity = _mapper.Map<Message>(createEntityDto);

                entity.ConversationId = (long)conversationId;

                entity.CreatedBy = currentUserId;

                await _dbContext.Messages.AddAsync(entity);

                int effectedRows = await _dbContext.SaveChangesAsync();

                if (!(effectedRows > 0))
                {
                    throw new Exception();
                }

                transaction.Commit();

                return _mapper.Map<MessageDto>(entity);
            }
            catch
            {
                transaction.Rollback();

                throw;
            }
        }

        public async Task<MessageDto> Update(
            UpdateMessageDto updateEntityDto, long currentUserId)
        {
            var entity = _dbContext.Messages.Find(updateEntityDto.Id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity = _mapper.Map(updateEntityDto, entity);

            entity.ModifiedDate = DateTime.UtcNow;

            entity.ModifiedBy = currentUserId;

            _dbContext.Messages.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<MessageDto>(entity);
        }

        public async Task<MessageDto> Remove(long id)
        {
            var entity = _dbContext.Messages.Find(id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity.IsDeleted = true;

            _dbContext.Messages.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<MessageDto>(entity);
        }

        private async Task<long?> GetConversationId(
            long senderId, long receiverId, long currentUserId)
        {
            var conversationId = _conversationService.GetBetweenId(senderId, receiverId);

            if (conversationId == null)
            {
                var conversation = await _conversationService.Create(
                    new CreateConversation
                    {
                        User1Id = senderId,
                        User2Id = receiverId,
                    },
                    currentUserId
                    );

                conversationId = conversation?.Id;
            }

            return conversationId;
        }
    }
}
