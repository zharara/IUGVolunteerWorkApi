using VolunteerWorkApi.Dtos.ManagementEmployee;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Services.ManagementEmployees
{
    public interface IManagementEmployeeService
    {
        IEnumerable<ManagementEmployeeDto> GetAll();

        IEnumerable<ManagementEmployeeDto> GetList(
            string? filter, int? skipCount, int? maxResultCount);

        ManagementEmployeeDto GetById(long id);

        Task<ManagementEmployeeAccount> Create(
            CreateManagementEmployeeDto createEntityDto);

        Task<ManagementEmployeeDto> Update(
            UpdateManagementEmployeeDto updateEntityDto, long currentUserId);

        Task<ManagementEmployeeDto> Remove(long id);
    }
}
