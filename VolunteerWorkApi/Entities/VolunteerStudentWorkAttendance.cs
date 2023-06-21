namespace VolunteerWorkApi.Entities
{
    public class VolunteerStudentWorkAttendance : BaseEntity
    {
        public bool IsAttended { get; set; }

        public long? VolunteerStudentId { get; set; }

        public VolunteerStudent VolunteerStudent { get; set; }

        public long VolunteerProgramWorkDayId { get; set; }

        public VolunteerProgramWorkDay VolunteerProgramWorkDay { get; set; }
    }
}
