using VolunteerWorkApi.Dtos.VolunteerProgramActivity;

namespace VolunteerWorkApi.Services.VolunteerProgramActivities
{
    public interface IVolunteerProgramActivityService
    {
        IEnumerable<VolunteerProgramActivityDto> GetAll();

        IEnumerable<VolunteerProgramActivityDto> GetList(
            string? filter, int? skipCount,
            int? maxResultCount, long? volunteerProgramId);

        VolunteerProgramActivityDto GetById(long id);

        Task<VolunteerProgramActivityDto> Create(
            CreateVolunteerProgramActivityDto createEntityDto, long currentUserId);

        Task<VolunteerProgramActivityDto> Update(
            UpdateVolunteerProgramActivityDto updateEntityDto, long currentUserId);

        Task<VolunteerProgramActivityDto> Remove(long id);
    }
}
