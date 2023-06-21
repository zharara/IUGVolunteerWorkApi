using VolunteerWorkApi.Dtos.VolunteerProgramWorkDay;
using VolunteerWorkApi.Dtos.VolunteerStudent;

namespace VolunteerWorkApi.Dtos.VolunteerStudentWorkAttendance
{
    public record VolunteerStudentWorkAttendanceDto
    {
        public long Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsAttended { get; set; }

        public long? VolunteerStudentId { get; set; }

        public VolunteerStudentDto VolunteerStudent { get; set; }

        public long VolunteerProgramWorkDayId { get; set; }

        public VolunteerProgramWorkDayDto VolunteerProgramWorkDay { get; set; }
    }
}
