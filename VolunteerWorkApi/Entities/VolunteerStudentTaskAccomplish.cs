namespace VolunteerWorkApi.Entities
{
    public class VolunteerStudentTaskAccomplish : BaseEntity
    {
        public bool IsAccomplished { get; set; }

        public double Rate { get; set; }

        public long? VolunteerStudentId { get; set; }

        public VolunteerStudent VolunteerStudent { get; set; }

        public long VolunteerProgramTaskId { get; set; }

        public VolunteerProgramTask VolunteerProgramTask { get; set; }
    }
}
