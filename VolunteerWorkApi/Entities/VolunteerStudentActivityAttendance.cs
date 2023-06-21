namespace VolunteerWorkApi.Entities
{
    public class VolunteerStudentActivityAttendance : BaseEntity
    {
        public bool IsAttended { get; set; }

        public long? VolunteerStudentId { get; set; }

        public VolunteerStudent VolunteerStudent { get; set; }

        public long VolunteerProgramActivityId { get; set; }

        public VolunteerProgramActivity VolunteerProgramActivity { get; set; }
    }
}
