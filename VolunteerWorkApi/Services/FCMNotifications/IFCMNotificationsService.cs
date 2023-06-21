using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Services.FCMNotifications
{
    public interface IFCMNotificationsService
    {
        void SendNotification(FCMNotification fcmNotification);
    }
}
