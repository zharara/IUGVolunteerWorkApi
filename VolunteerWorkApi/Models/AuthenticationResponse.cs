using VolunteerWorkApi.Dtos.Student;

namespace VolunteerWorkApi.Models
{
    public record AuthenticationResponse(
       ApplicationUser ApplicationUser,
       AuthToken AuthToken);
}
