namespace VolunteerWorkApi.Models
{
    public record ApiError
    {
        public string ErrorTitle { get; set; }
        public string ErrorMessage { get; set; }
        public string? ErrorDetails { get; set; }
    }
}
