using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Dtos.ManagementEmployee;
using VolunteerWorkApi.Helpers;
using VolunteerWorkApi.Models;
using VolunteerWorkApi.Services.ManagementEmployees;

namespace VolunteerWorkApi.Controllers
{
    [Authorize(Roles = UsersRoles.Management)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ManagementEmployeesController : ControllerBase
    {
        private readonly IManagementEmployeeService _managementEmployeeService;

        public ManagementEmployeesController(
            IManagementEmployeeService managementEmployeeService)
        {
            _managementEmployeeService = managementEmployeeService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ManagementEmployeeDto>),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public IActionResult GetAll()
        {
            IEnumerable<ManagementEmployeeDto> data =
                _managementEmployeeService.GetAll();

            return Ok(data);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ManagementEmployeeDto>),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public IActionResult GetList(
            string? filter, int? skipCount, int? maxResultCount)
        {
            IEnumerable<ManagementEmployeeDto> data =
                _managementEmployeeService
                .GetList(filter: filter, skipCount: skipCount,
                maxResultCount: maxResultCount);

            return Ok(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ManagementEmployeeDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]

        public IActionResult Get(long id)
        {
            ManagementEmployeeDto data =
                _managementEmployeeService.GetById(id: id);

            return Ok(data);
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(ManagementEmployeeAccount),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> Create(
            [FromBody] CreateManagementEmployeeDto createEntityDto)
        {
            ManagementEmployeeAccount data =
                await _managementEmployeeService.Create(createEntityDto);

            return Ok(data);
        }

        [HttpPut]
        [ProducesResponseType(typeof(ManagementEmployeeDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]

        public async Task<IActionResult> Update(
           [FromBody] UpdateManagementEmployeeDto updateEntityDto)
        {
            var userId = ParsingHelpers.ParseUserId(
                HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == null)
            {
                return Unauthorized();
            }

            ManagementEmployeeDto data =
                await _managementEmployeeService.Update(
                updateEntityDto, (long)userId);

            return Ok(data);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ManagementEmployeeDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]

        public async Task<IActionResult> Delete(long id)
        {
            ManagementEmployeeDto data =
                await _managementEmployeeService.Remove(id);

            return Ok(data);
        }
    }
}
