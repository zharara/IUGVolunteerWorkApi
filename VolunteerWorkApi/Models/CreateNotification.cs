using VolunteerWorkApi.Enums;

namespace VolunteerWorkApi.Models
{
    public record CreateNotification
    {
        public string Title { get; set; }

        public string Body { get; set; }

        public long? ItemId { get; set; }

        public NotificationPage Page { get; set; } = NotificationPage.Notifications;

        public long ApplicationUserId { get; set; }
    }
}
