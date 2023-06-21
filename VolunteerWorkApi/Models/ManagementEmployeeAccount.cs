using VolunteerWorkApi.Dtos.ManagementEmployee;

namespace VolunteerWorkApi.Models
{
    public record ManagementEmployeeAccount(
        ManagementEmployeeDto ManagementEmployeeDto,
        AuthToken AuthToken);
}
