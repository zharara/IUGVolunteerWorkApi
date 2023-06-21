
using VolunteerWorkApi.Dtos.VolunteerProgramWorkDay;

namespace VolunteerWorkApi.Services.VolunteerProgramWorkDays
{
    public interface IVolunteerProgramWorkDayService
    {
        IEnumerable<VolunteerProgramWorkDayDto> GetAll();

        IEnumerable<VolunteerProgramWorkDayDto> GetList(
            int? skipCount, int? maxResultCount, long? volunteerProgramId,
            DateTime? startDate, DateTime? endDate);

        VolunteerProgramWorkDayDto GetById(long id);

        Task<VolunteerProgramWorkDayDto> Create(
            CreateVolunteerProgramWorkDayDto createEntityDto, long currentUserId);

        Task<VolunteerProgramWorkDayDto> Update(
            UpdateVolunteerProgramWorkDayDto updateEntityDto, long currentUserId);

        Task<VolunteerProgramWorkDayDto> Remove(long id);
    }
}
