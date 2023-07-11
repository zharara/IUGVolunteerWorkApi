using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Dtos.Student;
using VolunteerWorkApi.Helpers;
using VolunteerWorkApi.Models;
using VolunteerWorkApi.Services.Students;

namespace VolunteerWorkApi.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(
            IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<StudentDto>),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public IActionResult GetAll()
        {
            IEnumerable<StudentDto> data =
                _studentService.GetAll();

            return Ok(data);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<StudentDto>),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public IActionResult GetList(
            string? filter, bool? isNotVolunteer,
            int? skipCount, int? maxResultCount)
        {
            IEnumerable<StudentDto> data =
                _studentService
                .GetList(filter: filter, isNotVolunteer: isNotVolunteer,
                skipCount: skipCount, maxResultCount: maxResultCount);

            return Ok(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(StudentDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]

        public IActionResult Get(long id)
        {
            StudentDto data =
                _studentService.GetById(id: id);

            return Ok(data);
        }

        [Authorize(Roles = UsersRoles.Management)]
        [HttpPost]
        [ProducesResponseType(typeof(StudentAccount),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> Create(
            [FromBody] CreateStudentDto createEntityDto)
        {
            var userId = ParsingHelpers.ParseUserId(
                    HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == null)
            {
                return Unauthorized();
            }

            StudentAccount data =
                await _studentService.Create(createEntityDto, (long)userId);

            return Ok(data);
        }

        [Authorize(Roles = $"{UsersRoles.Management}, {UsersRoles.Student}")]
        [HttpPut]
        [ProducesResponseType(typeof(StudentDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]

        public async Task<IActionResult> Update(
           [FromBody] UpdateStudentDto updateEntityDto)
        {
            var userId = ParsingHelpers.ParseUserId(
                    HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == null)
            {
                return Unauthorized();
            }

            StudentDto data =
                await _studentService.Update(
                updateEntityDto, (long)userId);

            return Ok(data);
        }

        [Authorize(Roles = UsersRoles.Management)]
        [HttpPut]
        [ProducesResponseType(typeof(StudentDto),
    (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]

        public async Task<IActionResult> UpdateByManagement(
            [FromBody] UpdateStudentByManagementDto updateStudentByManagementDto)
        {
            var userId = ParsingHelpers.ParseUserId(
                    HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == null)
            {
                return Unauthorized();
            }

            StudentDto data =
                await _studentService.UpdateByManagement(
                updateStudentByManagementDto, (long)userId);

            return Ok(data);
        }

        [Authorize(Roles = $"{UsersRoles.Management}, {UsersRoles.Student}")]
        [HttpPut]
        [ProducesResponseType(typeof(StudentDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]

        public async Task<IActionResult> UpdateSkills(
            [FromBody] UpdateStudentSkills updateStudentSkills)
        {
            var userId = ParsingHelpers.ParseUserId(
                    HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == null)
            {
                return Unauthorized();
            }

            StudentDto data =
                await _studentService.UpdateSkills(
                updateStudentSkills, (long)userId);

            return Ok(data);
        }

        [Authorize(Roles = UsersRoles.Management)]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(StudentDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]

        public async Task<IActionResult> Delete(long id)
        {
            StudentDto data =
                await _studentService.Remove(id);

            return Ok(data);
        }
    }
}
