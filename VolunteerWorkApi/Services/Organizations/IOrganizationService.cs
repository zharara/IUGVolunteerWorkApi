using VolunteerWorkApi.Dtos.Organization;
using VolunteerWorkApi.Entities;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Services.Organizations
{
    public interface IOrganizationService
    {
        IEnumerable<OrganizationDto> GetAll();

        IEnumerable<OrganizationDto> GetList(
            string? filter, int? skipCount, int? maxResultCount);

        OrganizationDto GetById(long id);

        Task<OrganizationAccount> Create(
            CreateOrganizationDto createEntityDto);

        Task<OrganizationDto> Update(
            UpdateOrganizationDto updateEntityDto, long currentUserId);

        Task<OrganizationDto> Remove(long id);
    }
}
