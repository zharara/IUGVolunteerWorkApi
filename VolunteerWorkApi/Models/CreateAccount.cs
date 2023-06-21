
namespace VolunteerWorkApi.Models
{
    public record CreateAccount(
        ApplicationUser ApplicationUser,
        string Password);
}
