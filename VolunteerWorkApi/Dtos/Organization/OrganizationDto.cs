using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Dtos.ApplicationUser;

namespace VolunteerWorkApi.Dtos.Organization
{
    public record OrganizationDto : ApplicationUserDto
    {
        public string OrgNameLocal { get; set; }

        public string OrgNameForeign { get; set; }

        public string FieldOfWork { get; set; }

        public string About { get; set; }

        public string? Vision { get; set; }

        public string? Mission { get; set; }

        public string? Address { get; set; }
    }
}
