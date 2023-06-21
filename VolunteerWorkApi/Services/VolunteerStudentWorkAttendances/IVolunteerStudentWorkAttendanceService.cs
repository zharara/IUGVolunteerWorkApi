
using VolunteerWorkApi.Dtos.VolunteerStudentWorkAttendance;

namespace VolunteerWorkApi.Services.VolunteerStudentWorkAttendances
{
    public interface IVolunteerStudentWorkAttendanceService
    {
        IEnumerable<VolunteerStudentWorkAttendanceDto> GetAll();

        IEnumerable<VolunteerStudentWorkAttendanceDto> GetList(
            string? filter, int? skipCount, int? maxResultCount,
            long? volunteerStudentId, long? workDayId, bool? isAttended);

        VolunteerStudentWorkAttendanceDto GetById(long id);

        Task<VolunteerStudentWorkAttendanceDto> Create(
            CreateVolunteerStudentWorkAttendanceDto createEntityDto,
            long currentUserId);

        Task<VolunteerStudentWorkAttendanceDto> Update(
            UpdateVolunteerStudentWorkAttendanceDto updateEntityDto,
            long currentUserId);

        Task<VolunteerStudentWorkAttendanceDto> Remove(long id);
    }
}
