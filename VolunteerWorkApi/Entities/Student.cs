
namespace VolunteerWorkApi.Entities
{
    public class Student : ApplicationUser
    {
        public int UniversityIdNumber { get; set; }

        public string? Specialization { get; set; }

        public string? Address { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Biography { get; set; }

        public bool IsEnrolledInProgram { get; set; }

        public ICollection<Skill> Skills { get; set; }
    }
}
