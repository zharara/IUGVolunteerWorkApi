using VolunteerWorkApi.Enums;

namespace VolunteerWorkApi.Entities
{
    public class Notification : BaseEntity
    {
        public string Title { get; set; }

        public string Body { get; set; }

        public long? ItemId { get; set; }

        public NotificationPage Page { get; set; }

        public long ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public bool IsRead { get; set; } = false;
    }
}
