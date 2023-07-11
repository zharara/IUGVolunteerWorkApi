using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Dtos.VolunteerStudent;
using VolunteerWorkApi.Helpers;
using VolunteerWorkApi.Models;
using VolunteerWorkApi.Services.VolunteerStudents;

namespace VolunteerWorkApi.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VolunteerStudentsController : ControllerBase
    {
        private readonly IVolunteerStudentService _volunteerStudentService;

        public VolunteerStudentsController(
           IVolunteerStudentService volunteerStudentService)
        {
            _volunteerStudentService = volunteerStudentService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<VolunteerStudentDto>),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public IActionResult GetAll()
        {
            IEnumerable<VolunteerStudentDto> data =
                _volunteerStudentService.GetAll();

            return Ok(data);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<VolunteerStudentDto>),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public IActionResult GetList(
            string? filter, int? skipCount,
            int? maxResultCount, long? volunteerProgramId,
            long? organizationId)
        {
            IEnumerable<VolunteerStudentDto> data =
                _volunteerStudentService
                .GetList(filter: filter, skipCount: skipCount,
                maxResultCount: maxResultCount,
                volunteerProgramId: volunteerProgramId,
                organizationId: organizationId);

            return Ok(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(VolunteerStudentDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]

        public IActionResult Get(long id)
        {
            VolunteerStudentDto data =
                _volunteerStudentService.GetById(id: id);

            return Ok(data);
        }

        [HttpGet("{studentId}")]
        [ProducesResponseType(typeof(VolunteerStudentDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]

        public IActionResult GetOfStudentById(long studentId)
        {
            VolunteerStudentDto? data =
                _volunteerStudentService.GetOfStudentById(studentId: studentId);

            if (data == null)
            {
                return NoContent();
            }

            return Ok(data);
        }

        [Authorize(Roles = $"{UsersRoles.Management}, {UsersRoles.Organization}")]
        [HttpPost]
        [ProducesResponseType(typeof(VolunteerStudentDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> Create(
            [FromBody] CreateVolunteerStudentDto createEntityDto)
        {
            var userId = ParsingHelpers.ParseUserId(
                HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == null)
            {
                return Unauthorized();
            }


            VolunteerStudentDto data =
                await _volunteerStudentService
                .Create(createEntityDto, (long)userId);

            return Ok(data);
        }

        [Authorize(Roles = UsersRoles.Organization)]
        [HttpPost]
        [ProducesResponseType(typeof(VolunteerStudentDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> UpdateOrganizationAssessment(
           [FromBody] UpdateVolunteerStudentOrgAssessmentDto updateEntityDto)
        {
            var userId = ParsingHelpers.ParseUserId(
                HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == null)
            {
                return Unauthorized();
            }

            VolunteerStudentDto data =
                await _volunteerStudentService.UpdateOrganizationAssessment(
                updateEntityDto, (long)userId);

            return Ok(data);
        }

        [Authorize(Roles = UsersRoles.Management)]
        [HttpPost]
        [ProducesResponseType(typeof(VolunteerStudentDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> UpdateManagementFinalEvaluation(
            [FromBody] UpdateVolunteerStudentFinalEvaluationDto updateEntityDto)
        {
            var userId = ParsingHelpers.ParseUserId(
                HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == null)
            {
                return Unauthorized();
            }

            VolunteerStudentDto data =
                await _volunteerStudentService.UpdateManagementFinalEvaluation(
                updateEntityDto, (long)userId);

            return Ok(data);
        }

        [Authorize(Roles = $"{UsersRoles.Management}, {UsersRoles.Organization}")]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(VolunteerStudentDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]

        public async Task<IActionResult> Delete(long id)
        {
            VolunteerStudentDto data =
                await _volunteerStudentService.Remove(id);

            return Ok(data);
        }
    }
}
