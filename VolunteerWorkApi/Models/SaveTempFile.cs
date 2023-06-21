using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;

namespace VolunteerWorkApi.Models
{
    public class SaveTempFile
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long TempFileId { get; set; }
    }
}
