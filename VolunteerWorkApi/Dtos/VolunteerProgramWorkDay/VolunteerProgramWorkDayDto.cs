namespace VolunteerWorkApi.Dtos.VolunteerProgramWorkDay
{
    public record VolunteerProgramWorkDayDto
    {
        public long Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public long VolunteerProgramId { get; set; }
    }
}
