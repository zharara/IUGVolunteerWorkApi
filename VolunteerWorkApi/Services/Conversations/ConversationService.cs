using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Data;
using VolunteerWorkApi.Dtos.Conversation;
using VolunteerWorkApi.Extensions;
using VolunteerWorkApi.Helpers.ErrorHandling;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Services.Conversations
{
    public class ConversationService : IConversationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public ConversationService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IEnumerable<ConversationDto> GetAll(long currentUserId)
        {
            return _dbContext.Conversations
                .Where(x => x.User1Id == currentUserId
                    || x.User2Id == currentUserId)
                .Include(x => x.User1)
                .ThenInclude(x => x.ProfilePicture)
                .Include(x => x.User2)
                .ThenInclude(x => x.ProfilePicture)
                .Select(x => _mapper.Map<ConversationDto>(x))
                .ToList();
        }

        public IEnumerable<ConversationDto> GetList(long currentUserId,
             int? skipCount, int? maxResultCount, long? senderId, long? recieverId)
        {
            return _dbContext.Conversations
                 .Where(x => x.User1Id == currentUserId
                    || x.User2Id == currentUserId)
                 .WhereIf(senderId != null,
                    x => x.User1Id == senderId || x.User2Id == senderId)
                 .WhereIf(recieverId != null,
                    x => x.User1Id == recieverId || x.User2Id == recieverId)
                 .Skip(skipCount ?? 0)
                 .Take(maxResultCount ?? ApiConstants.MaxResultCount)
                 .Include(x => x.User1)
                 .ThenInclude(x => x.ProfilePicture)
                 .Include(x => x.User2)
                 .ThenInclude(x => x.ProfilePicture)
                 .Select(x => _mapper.Map<ConversationDto>(x))
                 .ToList();
        }

        public ConversationDto? GetBetween(long user1Id, long user2Id)
        {
            return _dbContext.Conversations
                .Where(x =>
                (x.User1Id == user1Id && x.User2Id == user2Id)
                || (x.User1Id == user2Id && x.User2Id == user1Id))
                .Include(x => x.User1)
                .ThenInclude(x => x.ProfilePicture)
                .Include(x => x.User2)
                .ThenInclude(x => x.ProfilePicture)
                .Select(x => _mapper.Map<ConversationDto>(x))
                .FirstOrDefault();
        }

        public long? GetBetweenId(long user1Id, long user2Id)
        {
            var data = _dbContext.Conversations
                .Where(x =>
                (x.User1Id == user1Id && x.User2Id == user2Id)
                || (x.User1Id == user2Id && x.User2Id == user1Id))
                .Select(x => new
                {
                    x.Id,
                })
                .FirstOrDefault();

            return data?.Id;
        }

        public ConversationDto GetById(long id)
        {
            var data = _dbContext.Conversations.Find(id);

            if (data == null)
            {
                throw new ApiNotFoundException();
            }

            return _mapper.Map<ConversationDto>(data);
        }

        public Conversation? GetEntityById(long id)
        {
            return _dbContext.Conversations.Find(id);
        }

        public async Task<ConversationDto> Create(
            CreateConversation createConversation, long currentUserId)
        {
            var entity = _mapper.Map<Conversation>(createConversation);

            entity.CreatedBy = currentUserId;

            var addedEntity = await _dbContext.Conversations.AddAsync(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            var item = _dbContext.Conversations
                        .Include(x => x.User1)
                        .ThenInclude(x => x.ProfilePicture)
                        .Include(x => x.User2)
                        .ThenInclude(x => x.ProfilePicture)
                        .Where(x => x.Id == addedEntity.Entity.Id)
                        .FirstOrDefault();

            return _mapper.Map<ConversationDto>(item);
        }

        public async Task<ConversationDto> Remove(long id)
        {
            var entity = _dbContext.Conversations.Find(id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity.IsDeleted = true;

            _dbContext.Conversations.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<ConversationDto>(entity);
        }
    }
}
