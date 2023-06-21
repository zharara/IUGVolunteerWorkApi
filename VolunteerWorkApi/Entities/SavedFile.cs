using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VolunteerWorkApi.Entities
{
    public class SavedFile
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public string FileKey { get; set; }

        public string OriginalFileName { get; set; }

        public long FileSize { get; set; }

        public string FileExtension { get; set; }
    }
}
