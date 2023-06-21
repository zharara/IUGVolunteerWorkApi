using AutoMapper;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Data;
using VolunteerWorkApi.Dtos.Notification;
using VolunteerWorkApi.Extensions;
using VolunteerWorkApi.Helpers.ErrorHandling;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Services.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public NotificationService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IEnumerable<NotificationDto> GetList(long userId,
            string? filter, int? skipCount,
            int? maxResultCount, bool? isRead,
            DateTime? startDate, DateTime? endDate)
        {
            return _dbContext.Notifications
                 .Where(x => x.ApplicationUserId == userId)
                 .WhereIf(!string.IsNullOrEmpty(filter),
                    x => x.Title.Contains(filter!) 
                    || x.Body.Contains(filter!))
                 .WhereIf(isRead != null,
                    x => x.IsRead == isRead)
                 .WhereIf(startDate is DateTime,
                    x => x.CreatedDate.Date >= ((DateTime)startDate!).Date)
                 .WhereIf(endDate is DateTime,
                    x => x.CreatedDate.Date <= ((DateTime)endDate!).Date)
                 .Skip(skipCount ?? 0)
                 .Take(maxResultCount ?? ApiConstants.MaxResultCount)
                 .Select(x => _mapper.Map<NotificationDto>(x))
                 .ToList();
        }

        public async Task<Notification> Create(
            CreateNotification createNotification)
        {
            var entity = _mapper.Map<Notification>(createNotification);

            await _dbContext.Notifications.AddAsync(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return entity;
        }

        public async Task<NotificationDto> MarkNotificationAsRead(long id)
        {
            var entity = _dbContext.Notifications.Find(id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity.IsRead = true;

            entity.ModifiedDate = DateTime.UtcNow;

            _dbContext.Notifications.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<NotificationDto>(entity);
        }
    }
}
