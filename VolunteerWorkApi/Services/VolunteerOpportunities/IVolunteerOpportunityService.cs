using VolunteerWorkApi.Dtos.VolunteerOpportunity;

namespace VolunteerWorkApi.Services.VolunteerOpportunities
{
    public interface IVolunteerOpportunityService
    {
        IEnumerable<VolunteerOpportunityDto> GetAll();

        IEnumerable<VolunteerOpportunityDto> GetList(
            string? filter, int? skipCount,
            int? maxResultCount, long? organizationId);

        VolunteerOpportunityDto GetById(long id);

        Task<VolunteerOpportunityDto> Create(
            CreateVolunteerOpportunityDto createEntityDto, long currentUserId);

        Task<VolunteerOpportunityDto> Update(
            UpdateVolunteerOpportunityDto updateEntityDto, long currentUserId);

        Task<VolunteerOpportunityDto> Remove(long id);    
    }
}
