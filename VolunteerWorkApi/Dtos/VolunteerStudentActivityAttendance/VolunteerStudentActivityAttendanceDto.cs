using VolunteerWorkApi.Dtos.VolunteerProgramActivity;
using VolunteerWorkApi.Dtos.VolunteerStudent;

namespace VolunteerWorkApi.Dtos.VolunteerStudentActivityAttendance
{
    public record VolunteerStudentActivityAttendanceDto
    {
        public long Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsAttended { get; set; }

        public long? VolunteerStudentId { get; set; }

        public VolunteerStudentDto VolunteerStudent { get; set; }

        public long VolunteerProgramActivityId { get; set; }

        public VolunteerProgramActivityDto VolunteerProgramActivity { get; set; }
    }
}
