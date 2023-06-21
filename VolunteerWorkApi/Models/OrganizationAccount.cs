using VolunteerWorkApi.Dtos.Organization;

namespace VolunteerWorkApi.Models
{
    public record OrganizationAccount(
       OrganizationDto OrganizationDto,
       AuthToken AuthToken);
}
