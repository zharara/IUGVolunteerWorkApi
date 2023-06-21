using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;

namespace VolunteerWorkApi.Models
{
    public record RejectStudentApplication
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long StudentApplicationId { get; set; }

        public string? RejectionReason { get; set; }
    }
}
