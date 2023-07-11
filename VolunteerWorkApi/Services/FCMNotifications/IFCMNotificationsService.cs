using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Services.FCMNotifications
{
    public interface IFCMNotificationsService
    {
        Task SendNotification(FCMNotification fcmNotification);
    }
}
