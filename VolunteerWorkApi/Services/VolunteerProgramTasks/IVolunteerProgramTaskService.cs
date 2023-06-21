
using VolunteerWorkApi.Dtos.VolunteerProgramTask;

namespace VolunteerWorkApi.Services.VolunteerProgramTasks
{
    public interface IVolunteerProgramTaskService
    {
        IEnumerable<VolunteerProgramTaskDto> GetAll();

        IEnumerable<VolunteerProgramTaskDto> GetList(
            string? filter, int? skipCount,
            int? maxResultCount, long? volunteerProgramId);

        VolunteerProgramTaskDto GetById(long id);

        Task<VolunteerProgramTaskDto> Create(
            CreateVolunteerProgramTaskDto createEntityDto, long currentUserId);

        Task<VolunteerProgramTaskDto> Update(
            UpdateVolunteerProgramTaskDto updateEntityDto, long currentUserId);

        Task<VolunteerProgramTaskDto> Remove(long id);
    }
}
