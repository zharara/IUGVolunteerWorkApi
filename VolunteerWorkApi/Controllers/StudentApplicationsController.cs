using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Dtos.StudentApplication;
using VolunteerWorkApi.Helpers;
using VolunteerWorkApi.Models;
using VolunteerWorkApi.Services.StudentApplications;

namespace VolunteerWorkApi.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentApplicationsController : ControllerBase
    {
        private readonly IStudentApplicationService _studentApplicationService;

        public StudentApplicationsController(
            IStudentApplicationService studentApplicationService)
        {
            _studentApplicationService = studentApplicationService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<StudentApplicationDto>),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public IActionResult GetAll()
        {
            IEnumerable<StudentApplicationDto> data =
                _studentApplicationService.GetAll();

            return Ok(data);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<StudentApplicationDto>),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public IActionResult GetList(
           int? skipCount, int? maxResultCount,
           long? studentId, long? volunteerOpportunityId,
           long? organizationId)
        {
            IEnumerable<StudentApplicationDto> data =
                _studentApplicationService
                .GetList(skipCount: skipCount,
                maxResultCount: maxResultCount,
                studentId: studentId,
                volunteerOpportunityId: volunteerOpportunityId,
                organizationId: organizationId);

            return Ok(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(StudentApplicationDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]

        public IActionResult Get(long id)
        {
            StudentApplicationDto data =
                _studentApplicationService.GetById(id: id);

            return Ok(data);
        }

        [Authorize(Roles = UsersRoles.Organization)]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(StudentApplicationDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]

        public async Task<IActionResult> OrganizationAcceptApplication(long id)
        {
            var userId = ParsingHelpers.ParseUserId(
                HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == null)
            {
                return Unauthorized();
            }

            StudentApplicationDto data =
                await _studentApplicationService.OrganizationAcceptApplication(
                studentApplicationId: id, (long)userId);

            return Ok(data);
        }

        [Authorize(Roles = UsersRoles.Organization)]
        [HttpPut]
        [ProducesResponseType(typeof(StudentApplicationDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]

        public async Task<IActionResult> OrganizationRejectApplication(
            [FromBody] RejectStudentApplication rejectStudentApplicationDto)
        {
            var userId = ParsingHelpers.ParseUserId(
                HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == null)
            {
                return Unauthorized();
            }

            StudentApplicationDto data =
                await _studentApplicationService.OrganizationRejectApplication(
                rejectStudentApplicationDto, (long)userId);

            return Ok(data);
        }

        [Authorize(Roles = UsersRoles.Management)]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(StudentApplicationDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]

        public async Task<IActionResult> ManagementAcceptApplication(long id)
        {
            var userId = ParsingHelpers.ParseUserId(
                HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == null)
            {
                return Unauthorized();
            }

            StudentApplicationDto data =
                await _studentApplicationService.ManagementAcceptApplication(
                studentApplicationId: id, (long)userId);

            return Ok(data);
        }

        [Authorize(Roles = UsersRoles.Management)]
        [HttpPut]
        [ProducesResponseType(typeof(StudentApplicationDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]

        public async Task<IActionResult> ManagementRejectApplication(
            [FromBody] RejectStudentApplication rejectStudentApplicationDto)
        {
            var userId = ParsingHelpers.ParseUserId(
                HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == null)
            {
                return Unauthorized();
            }

            StudentApplicationDto data =
                await _studentApplicationService.ManagementRejectApplication(
                rejectStudentApplicationDto, (long)userId);

            return Ok(data);
        }

        [HttpPost]
        [ProducesResponseType(typeof(StudentApplicationDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> Create(
            [FromBody] CreateStudentApplicationDto createEntityDto)
        {
            var userId = ParsingHelpers.ParseUserId(
                HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == null)
            {
                return Unauthorized();
            }

            StudentApplicationDto data =
                await _studentApplicationService
                .Create(createEntityDto, (long)userId);

            return Ok(data);
        }

        [HttpPut]
        [ProducesResponseType(typeof(StudentApplicationDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]

        public async Task<IActionResult> Update(
           [FromBody] UpdateStudentApplicationDto updateEntityDto)
        {
            var userId = ParsingHelpers.ParseUserId(
                HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == null)
            {
                return Unauthorized();
            }

            StudentApplicationDto data =
                await _studentApplicationService.Update(
                updateEntityDto, (long)userId);

            return Ok(data);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(StudentApplicationDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]

        public async Task<IActionResult> Delete(long id)
        {
            StudentApplicationDto data =
                await _studentApplicationService.Remove(id);

            return Ok(data);
        }
    }
}
