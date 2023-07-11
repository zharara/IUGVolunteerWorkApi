using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Services.FCMNotifications
{
    public class FCMNotificationsService : IFCMNotificationsService
    {
        private readonly ILogger<FCMNotificationsService> _logger;
        private readonly FirebaseApp _app;

        public FCMNotificationsService(
            IWebHostEnvironment env,
            ILogger<FCMNotificationsService> logger)
        {
            _logger = logger;

            try
            {
                var path = Path.Combine(
                    env.ContentRootPath, "firebase-key.json");

                _app = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(path)
                }, "iugvw");
            }
            catch
            {
                _app = FirebaseApp.GetInstance("iugvw");
            }
        }

        public async Task SendNotification(FCMNotification fcmNotification)
        {
            try
            {
                var fcm = FirebaseAdmin.Messaging.
                    FirebaseMessaging.GetMessaging(_app);
                FirebaseAdmin.Messaging.Message message = new FirebaseAdmin.Messaging.Message()
                {
                    Notification = new FirebaseAdmin.Messaging.Notification
                    {
                        Title = fcmNotification.Title,
                        Body = fcmNotification.Body,
                    },
                    Data = new Dictionary<string, string>()
                 {
                     { "id", fcmNotification.ItemId.ToString() ?? "" },
                     { "page", ((int)fcmNotification.Page).ToString() },
                 },

                    Token = fcmNotification.FCMToken
                };

                var result = await fcm.SendAsync(message);

                _logger.LogInformation(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
