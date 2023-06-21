using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Dtos.VolunteerProgram;
using VolunteerWorkApi.Helpers;
using VolunteerWorkApi.Models;
using VolunteerWorkApi.Services.VolunteerPrograms;

namespace VolunteerWorkApi.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VolunteerProgramsController : ControllerBase
    {
        private readonly IVolunteerProgramService _volunteerProgramService;

        public VolunteerProgramsController(
            IVolunteerProgramService volunteerProgramService)
        {
            _volunteerProgramService = volunteerProgramService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<VolunteerProgramDto>),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public IActionResult GetAll()
        {
            IEnumerable<VolunteerProgramDto> data =
                _volunteerProgramService.GetAll();

            return Ok(data);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<VolunteerProgramDto>),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public IActionResult GetList(
            string? filter, int? skipCount,
            int? maxResultCount, long? organizationId, bool? isInternalProgram)
        {
            IEnumerable<VolunteerProgramDto> data =
                _volunteerProgramService
                .GetList(filter: filter, skipCount: skipCount,
                maxResultCount: maxResultCount,
                organizationId: organizationId,
                isInternalProgram: isInternalProgram);

            return Ok(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(VolunteerProgramDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]

        public IActionResult Get(long id)
        {
            VolunteerProgramDto data =
                _volunteerProgramService.GetById(id: id);

            return Ok(data);
        }

        [Authorize(Roles = UsersRoles.Organization)]
        [HttpPost]
        [ProducesResponseType(typeof(VolunteerProgramDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> CreateOrganizationProgram(
            [FromBody] CreateVolunteerProgramDto createEntityDto)
        {
            var userId = ParsingHelpers.ParseUserId(
                HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == null)
            {
                return Unauthorized();
            }


            VolunteerProgramDto data =
                await _volunteerProgramService
                .Create(createEntityDto, (long)userId);

            return Ok(data);
        }

        [Authorize(Roles = UsersRoles.Management)]
        [HttpPost]
        [ProducesResponseType(typeof(VolunteerProgramDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> CreateInternalProgram(
            [FromBody] CreateInternalVolunteerProgramDto createEntityDto)
        {
            var userId = ParsingHelpers.ParseUserId(
                HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == null)
            {
                return Unauthorized();
            }


            VolunteerProgramDto data =
                await _volunteerProgramService
                .CreateInternalProgram(createEntityDto, (long)userId);

            return Ok(data);
        }

        [Authorize(Roles = UsersRoles.Organization)]
        [HttpPut]
        [ProducesResponseType(typeof(VolunteerProgramDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]

        public async Task<IActionResult> UpdateOrganizationProgram(
           [FromBody] UpdateVolunteerProgramDto updateEntityDto)
        {
            var userId = ParsingHelpers.ParseUserId(
                HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == null)
            {
                return Unauthorized();
            }

            VolunteerProgramDto data =
                await _volunteerProgramService.Update(
                updateEntityDto, (long)userId);

            return Ok(data);
        }

        [Authorize(Roles = UsersRoles.Management)]
        [HttpPut]
        [ProducesResponseType(typeof(VolunteerProgramDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]

        public async Task<IActionResult> UpdateInternalProgram(
            [FromBody] UpdateInternalVolunteerProgramDto updateEntityDto)
        {
            var userId = ParsingHelpers.ParseUserId(
                HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == null)
            {
                return Unauthorized();
            }

            VolunteerProgramDto data =
                await _volunteerProgramService.UpdateInternalProgram(
                updateEntityDto, (long)userId);

            return Ok(data);
        }

        [Authorize(Roles = $"{UsersRoles.Management}, {UsersRoles.Organization}")]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(VolunteerProgramDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]

        public async Task<IActionResult> Delete(long id)
        {
            VolunteerProgramDto data =
                await _volunteerProgramService.Remove(id);

            return Ok(data);
        }
    }
}
