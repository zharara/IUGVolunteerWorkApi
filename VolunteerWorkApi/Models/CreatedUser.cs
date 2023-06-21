namespace VolunteerWorkApi.Models
{
    public record CreatedUser(
        ApplicationUser ApplicationUser,
        AuthToken AuthToken);
}
