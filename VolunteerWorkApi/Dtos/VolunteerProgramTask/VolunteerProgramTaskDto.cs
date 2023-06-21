namespace VolunteerWorkApi.Dtos.VolunteerProgramTask
{
    public record VolunteerProgramTaskDto
    {
        public long Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public long VolunteerProgramId { get; set; }
    }
}
