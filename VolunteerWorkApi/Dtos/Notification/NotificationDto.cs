using VolunteerWorkApi.Enums;

namespace VolunteerWorkApi.Dtos.Notification
{
    public record NotificationDto
    {
        public long Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public long? ItemId { get; set; }

        public NotificationPage Page { get; set; }

        public long ApplicationUserId { get; set; }

        public bool IsRead { get; set; }
    }
}
