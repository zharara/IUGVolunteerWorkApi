
namespace VolunteerWorkApi.Entities
{
    public class Organization : ApplicationUser
    {
        public string FieldOfWork { get; set; }

        public string About { get; set; }

        public string? Vision { get; set; }

        public string? Mission { get; set; }

        public string? Address { get; set; }

        public ICollection<VolunteerOpportunity> VolunteerOpportunities { get; set; }

        public ICollection<VolunteerProgram> VolunteerPrograms { get; set; }
    }
}
