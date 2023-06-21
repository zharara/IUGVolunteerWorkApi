namespace VolunteerWorkApi.Entities
{
    public class VolunteerProgramTask : BaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public long VolunteerProgramId { get; set; }

        public VolunteerProgram VolunteerProgram { get; set; }
    }
}
