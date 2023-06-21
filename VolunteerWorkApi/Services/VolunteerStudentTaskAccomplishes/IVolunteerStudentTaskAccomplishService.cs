
using VolunteerWorkApi.Dtos.VolunteerStudentTaskAccomplish;

namespace VolunteerWorkApi.Services.VolunteerStudentTaskAccomplishes
{
    public interface IVolunteerStudentTaskAccomplishService
    {
        IEnumerable<VolunteerStudentTaskAccomplishDto> GetAll();

        IEnumerable<VolunteerStudentTaskAccomplishDto> GetList(
            string? filter, int? skipCount, int? maxResultCount,
            long? volunteerStudentId, long? taskId, bool? isAccomplished);

        VolunteerStudentTaskAccomplishDto GetById(long id);

        Task<VolunteerStudentTaskAccomplishDto> Create(
            CreateVolunteerStudentTaskAccomplishDto createEntityDto,
            long currentUserId);

        Task<VolunteerStudentTaskAccomplishDto> Update(
            UpdateVolunteerStudentTaskAccomplishDto updateEntityDto,
            long currentUserId);

        Task<VolunteerStudentTaskAccomplishDto> Remove(long id);
    }
}
