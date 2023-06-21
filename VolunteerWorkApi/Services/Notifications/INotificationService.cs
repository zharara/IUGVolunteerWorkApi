using VolunteerWorkApi.Dtos.Notification;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Services.Notifications
{
    public interface INotificationService
    {
        IEnumerable<NotificationDto> GetList(
            long userId,
            string? filter, int? skipCount,
            int? maxResultCount, bool? isRead,
            DateTime? startDate, DateTime? endDate);

        Task<Notification> Create(
            CreateNotification createNotification);

        Task<NotificationDto> MarkNotificationAsRead(long id);
    }
}
