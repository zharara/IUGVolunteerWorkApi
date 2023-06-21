namespace VolunteerWorkApi.Dtos.Category
{
    public record CategoryDto
    {
        public long Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Name { get; set; }
    }
}
