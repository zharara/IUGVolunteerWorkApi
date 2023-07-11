using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Dtos.ManagementEmployee;
using VolunteerWorkApi.Dtos.Organization;
using VolunteerWorkApi.Dtos.Student;
using VolunteerWorkApi.Enums;
using VolunteerWorkApi.Helpers.ErrorHandling;
using VolunteerWorkApi.Models;
using VolunteerWorkApi.Services.Users;

namespace VolunteerWorkApi.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;

        public AuthController(IUsersService usersService, IMapper mapper)
        {
            _usersService = usersService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(ManagementEmployeeAccount),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> LoginManagement(AuthenticateRequest authRequest)
        {
            var authResponse = await _usersService
                .Authenticate(authRequest, AccountType.Management);

            var dto = _mapper.Map<ManagementEmployeeDto>(
                authResponse.ApplicationUser);

            return Ok(new ManagementEmployeeAccount(dto, authResponse.AuthToken));
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(OrganizationAccount),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> LoginOrganization(AuthenticateRequest authRequest)
        {
            var authResponse = await _usersService
                .Authenticate(authRequest, AccountType.Organization);

            var dto = _mapper.Map<OrganizationDto>(
                authResponse.ApplicationUser);

            return Ok(new OrganizationAccount(dto, authResponse.AuthToken));
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(StudentAccount),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> LoginStudent(AuthenticateRequest authRequest)
        {
            var authResponse = await _usersService
                .Authenticate(authRequest, AccountType.Student);

            var dto = _mapper.Map<StudentDto>(
                authResponse.ApplicationUser);

            return Ok(new StudentAccount(dto, authResponse.AuthToken));
        }

        [HttpPost]
        [ProducesResponseType(typeof(AuthToken),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> ChangePassword(
            string currentUserPassword, string newUserPassword)
        {
            var userId =
                HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var authToken = await _usersService
                .ChangeUserPassword(
                currentUserId: userId,
                currentPassword: currentUserPassword,
                newPassword: newUserPassword);

            return Ok(authToken);
        }

        [Authorize(Roles = UsersRoles.Management)]
        [HttpPost]
        [ProducesResponseType(typeof(ApplicationUser),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> ResetStudentPasswordByManagement(
            long studentId, string newPassword)
        {
            var applicationUser = await _usersService
                .ResetUserPassword(
                userId: studentId,
                newPassword: newPassword);

            return Ok(applicationUser);
        }
    }
}
