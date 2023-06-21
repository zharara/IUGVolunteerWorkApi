using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VolunteerWorkApi.Entities
{
    public class TempFile
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public string FileKey { get; set; }

        public string OriginalFileName { get; set; }
    }
}
