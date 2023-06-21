using CorePush.Firebase;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Services.FCMNotifications
{
    public class FCMNotificationsService : IFCMNotificationsService
    {
        private readonly FirebaseSender _firebaseSender;

        public FCMNotificationsService()
        {
            string firebaseSettingsJson = File.ReadAllText(
                Path.Combine(Directory.GetCurrentDirectory(),
                "volunteerworkapp-02d75fe3aaaf.json"));

            HttpClient httpClient = new();

            _firebaseSender = new FirebaseSender(firebaseSettingsJson, httpClient);
        }

        public void SendNotification(FCMNotification fcmNotification)
        {
            var payload = new
            {
                message = new
                {
                    token = fcmNotification.FCMToken,
                    notification = new
                    {
                        title = fcmNotification.Title,
                        body = fcmNotification.Body,
                    },
                    data = new
                    {
                        id = fcmNotification.ItemId.ToString(),
                        page = ((int)fcmNotification.Page).ToString(),
                    },
                }
            };

            _firebaseSender.SendAsync(payload);
        }
    }
}
