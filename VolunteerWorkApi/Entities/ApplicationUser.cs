using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace VolunteerWorkApi.Entities
{
    public abstract class ApplicationUser : IdentityUser<long>
    {
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedDate { get; set; } = null;

        public long? CreatedBy { get; set; }

        public long? ModifiedBy { get; set; }

        public bool IsDeleted { get; set; } = false;

        public string FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string LastName { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string FullName { get; private set; }

        public SavedFile? ProfilePicture { get; set; }

        public string? FCMToken { get; set; }
    }
}
