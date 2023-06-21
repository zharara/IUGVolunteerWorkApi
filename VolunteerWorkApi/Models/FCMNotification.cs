using VolunteerWorkApi.Enums;

namespace VolunteerWorkApi.Models
{
    public record FCMNotification
    {
        public string FCMToken { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public long? ItemId { get; set; }

        public NotificationPage Page { get; set; } = NotificationPage.Notifications;
    }
}
