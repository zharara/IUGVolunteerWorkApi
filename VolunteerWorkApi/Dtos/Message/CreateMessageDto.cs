using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;

namespace VolunteerWorkApi.Dtos.Message
{
    public record CreateMessageDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long SenderId { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long ReceiverId { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public string Content { get; set; }

        public long? ConversationId { get; set; }
    }
}
