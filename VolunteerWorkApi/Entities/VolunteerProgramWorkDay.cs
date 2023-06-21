namespace VolunteerWorkApi.Entities
{
    public class VolunteerProgramWorkDay : BaseEntity
    {
        public string Name { get; set; }

        public DateTime Date { get; set; }

        public long VolunteerProgramId { get; set; }

        public VolunteerProgram VolunteerProgram { get; set; }
    }
}
