using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Dtos.VolunteerStudentWorkAttendance;
using VolunteerWorkApi.Helpers;
using VolunteerWorkApi.Models;
using VolunteerWorkApi.Services.VolunteerStudentWorkAttendances;

namespace VolunteerWorkApi.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VolunteerStudentWorkAttendancesController : ControllerBase
    {
        private readonly IVolunteerStudentWorkAttendanceService _volunteerStudentWorkAttendanceService;

        public VolunteerStudentWorkAttendancesController(
           IVolunteerStudentWorkAttendanceService volunteerStudentWorkAttendanceService)
        {
            _volunteerStudentWorkAttendanceService = volunteerStudentWorkAttendanceService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<VolunteerStudentWorkAttendanceDto>),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public IActionResult GetAll()
        {
            IEnumerable<VolunteerStudentWorkAttendanceDto> data =
                _volunteerStudentWorkAttendanceService.GetAll();

            return Ok(data);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<VolunteerStudentWorkAttendanceDto>),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public IActionResult GetList(
            string? filter, int? skipCount,
            int? maxResultCount, long? volunteerStudentId,
            long? workDayId, bool? isAttended)
        {
            IEnumerable<VolunteerStudentWorkAttendanceDto> data =
                _volunteerStudentWorkAttendanceService
                .GetList(filter: filter, skipCount: skipCount,
                maxResultCount: maxResultCount,
                volunteerStudentId: volunteerStudentId,
                workDayId: workDayId, isAttended: isAttended);

            return Ok(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(VolunteerStudentWorkAttendanceDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]

        public IActionResult Get(long id)
        {
            VolunteerStudentWorkAttendanceDto data =
                _volunteerStudentWorkAttendanceService.GetById(id: id);

            return Ok(data);
        }

        [Authorize(Roles = $"{UsersRoles.Management}, {UsersRoles.Organization}")]
        [HttpPost]
        [ProducesResponseType(typeof(VolunteerStudentWorkAttendanceDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> Create(
            [FromBody] CreateVolunteerStudentWorkAttendanceDto createEntityDto)
        {
            var userId = ParsingHelpers.ParseUserId(
                HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == null)
            {
                return Unauthorized();
            }


            VolunteerStudentWorkAttendanceDto data =
                await _volunteerStudentWorkAttendanceService
                .Create(createEntityDto, (long)userId);

            return Ok(data);
        }

        [Authorize(Roles = $"{UsersRoles.Management}, {UsersRoles.Organization}")]
        [HttpPost]
        [ProducesResponseType(typeof(VolunteerStudentWorkAttendanceDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> Update(
           [FromBody] UpdateVolunteerStudentWorkAttendanceDto updateEntityDto)
        {
            var userId = ParsingHelpers.ParseUserId(
                HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == null)
            {
                return Unauthorized();
            }

            VolunteerStudentWorkAttendanceDto data =
                await _volunteerStudentWorkAttendanceService.Update(
                updateEntityDto, (long)userId);

            return Ok(data);
        }

        [Authorize(Roles = $"{UsersRoles.Management}, {UsersRoles.Organization}")]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(VolunteerStudentWorkAttendanceDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]

        public async Task<IActionResult> Delete(long id)
        {
            VolunteerStudentWorkAttendanceDto data =
                await _volunteerStudentWorkAttendanceService.Remove(id);

            return Ok(data);
        }
    }
}
