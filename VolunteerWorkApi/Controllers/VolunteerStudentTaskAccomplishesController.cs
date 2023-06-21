using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Dtos.VolunteerStudentTaskAccomplish;
using VolunteerWorkApi.Helpers;
using VolunteerWorkApi.Models;
using VolunteerWorkApi.Services.VolunteerStudentTaskAccomplishes;

namespace VolunteerWorkApi.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VolunteerStudentTaskAccomplishesController : ControllerBase
    {
        private readonly IVolunteerStudentTaskAccomplishService _volunteerStudentTaskAccomplishService;

        public VolunteerStudentTaskAccomplishesController(
           IVolunteerStudentTaskAccomplishService volunteerStudentTaskAccomplishService)
        {
            _volunteerStudentTaskAccomplishService = volunteerStudentTaskAccomplishService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<VolunteerStudentTaskAccomplishDto>),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public IActionResult GetAll()
        {
            IEnumerable<VolunteerStudentTaskAccomplishDto> data =
                _volunteerStudentTaskAccomplishService.GetAll();

            return Ok(data);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<VolunteerStudentTaskAccomplishDto>),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public IActionResult GetList(
            string? filter, int? skipCount,
            int? maxResultCount, long? volunteerStudentId,
            long? taskId, bool? isAccomplished)
        {
            IEnumerable<VolunteerStudentTaskAccomplishDto> data =
                _volunteerStudentTaskAccomplishService
                .GetList(filter: filter, skipCount: skipCount,
                maxResultCount: maxResultCount,
                volunteerStudentId: volunteerStudentId,
                taskId: taskId, isAccomplished: isAccomplished);

            return Ok(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(VolunteerStudentTaskAccomplishDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]

        public IActionResult Get(long id)
        {
            VolunteerStudentTaskAccomplishDto data =
                _volunteerStudentTaskAccomplishService.GetById(id: id);

            return Ok(data);
        }

        [Authorize(Roles = $"{UsersRoles.Management}, {UsersRoles.Organization}")]
        [HttpPost]
        [ProducesResponseType(typeof(VolunteerStudentTaskAccomplishDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> Create(
            [FromBody] CreateVolunteerStudentTaskAccomplishDto createEntityDto)
        {
            var userId = ParsingHelpers.ParseUserId(
                HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == null)
            {
                return Unauthorized();
            }


            VolunteerStudentTaskAccomplishDto data =
                await _volunteerStudentTaskAccomplishService
                .Create(createEntityDto, (long)userId);

            return Ok(data);
        }

        [Authorize(Roles = $"{UsersRoles.Management}, {UsersRoles.Organization}")]
        [HttpPost]
        [ProducesResponseType(typeof(VolunteerStudentTaskAccomplishDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> Update(
           [FromBody] UpdateVolunteerStudentTaskAccomplishDto updateEntityDto)
        {
            var userId = ParsingHelpers.ParseUserId(
                HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == null)
            {
                return Unauthorized();
            }

            VolunteerStudentTaskAccomplishDto data =
                await _volunteerStudentTaskAccomplishService.Update(
                updateEntityDto, (long)userId);

            return Ok(data);
        }

        [Authorize(Roles = $"{UsersRoles.Management}, {UsersRoles.Organization}")]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(VolunteerStudentTaskAccomplishDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]

        public async Task<IActionResult> Delete(long id)
        {
            VolunteerStudentTaskAccomplishDto data =
                await _volunteerStudentTaskAccomplishService.Remove(id);

            return Ok(data);
        }
    }
}
