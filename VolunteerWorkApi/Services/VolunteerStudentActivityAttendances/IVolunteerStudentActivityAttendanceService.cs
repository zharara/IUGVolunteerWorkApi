
using VolunteerWorkApi.Dtos.VolunteerStudentActivityAttendance;

namespace VolunteerWorkApi.Services.VolunteerStudentActivityAttendances
{
    public interface IVolunteerStudentActivityAttendanceService
    {
        IEnumerable<VolunteerStudentActivityAttendanceDto> GetAll();

        IEnumerable<VolunteerStudentActivityAttendanceDto> GetList(
            string? filter, int? skipCount, int? maxResultCount,
            long? volunteerStudentId, long? activityId, bool? isAttended);

        VolunteerStudentActivityAttendanceDto GetById(long id);

        Task<VolunteerStudentActivityAttendanceDto> Create(
            CreateVolunteerStudentActivityAttendanceDto createEntityDto,
            long currentUserId);

        Task<VolunteerStudentActivityAttendanceDto> Update(
            UpdateVolunteerStudentActivityAttendanceDto updateEntityDto,
            long currentUserId);

        Task<VolunteerStudentActivityAttendanceDto> Remove(long id);
    }
}
