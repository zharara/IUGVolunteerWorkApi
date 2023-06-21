using VolunteerWorkApi.Dtos.Student;

namespace VolunteerWorkApi.Models
{
    public record StudentAccount(
       StudentDto StudentDto,
       AuthToken AuthToken);
}
