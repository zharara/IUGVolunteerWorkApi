using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Dtos.VolunteerProgramGalleryPhoto;
using VolunteerWorkApi.Helpers;
using VolunteerWorkApi.Models;
using VolunteerWorkApi.Services.VolunteerProgramGalleryPhotos;

namespace VolunteerWorkApi.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VolunteerProgramGalleryPhotosController : ControllerBase
    {
        private readonly IVolunteerProgramGalleryPhotoService _volunteerProgramGalleryPhoto;

        public VolunteerProgramGalleryPhotosController(
           IVolunteerProgramGalleryPhotoService volunteerProgramGalleryPhoto)
        {
            _volunteerProgramGalleryPhoto = volunteerProgramGalleryPhoto;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<VolunteerProgramGalleryPhotoDto>),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public IActionResult GetAll()
        {
            IEnumerable<VolunteerProgramGalleryPhotoDto> data =
                _volunteerProgramGalleryPhoto.GetAll();

            return Ok(data);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<VolunteerProgramGalleryPhotoDto>),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public IActionResult GetList(int? skipCount, int? maxResultCount,
            long? volunteerProgramId, bool? isApproved)
        {
            IEnumerable<VolunteerProgramGalleryPhotoDto> data =
                _volunteerProgramGalleryPhoto
                .GetList(skipCount: skipCount, maxResultCount: maxResultCount,
                volunteerProgramId: volunteerProgramId, isApproved: isApproved);

            return Ok(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(VolunteerProgramGalleryPhotoDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]

        public IActionResult Get(long id)
        {
            VolunteerProgramGalleryPhotoDto data =
                _volunteerProgramGalleryPhoto.GetById(id: id);

            return Ok(data);
        }

        [Authorize(Roles = $"{UsersRoles.Management}, {UsersRoles.Organization}")]
        [HttpPost]
        [ProducesResponseType(typeof(VolunteerProgramGalleryPhotoDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> Create(
            [FromBody] CreateGalleryPhotoDto createEntityDto)
        {
            var userId = ParsingHelpers.ParseUserId(
                HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == null)
            {
                return Unauthorized();
            }

            VolunteerProgramGalleryPhotoDto data =
                await _volunteerProgramGalleryPhoto
                .Create(createEntityDto, (long)userId);

            return Ok(data);
        }

        [Authorize(Roles = UsersRoles.Student)]
        [HttpPost]
        [ProducesResponseType(typeof(VolunteerProgramGalleryPhotoDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> CreateByStudent(
            [FromBody] CreateGalleryPhotoByStudentDto createEntityDto)
        {
            var userId = ParsingHelpers.ParseUserId(
                HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == null)
            {
                return Unauthorized();
            }

            VolunteerProgramGalleryPhotoDto data =
                await _volunteerProgramGalleryPhoto
                .CreateGalleryPhotoByStudent(createEntityDto, (long)userId);

            return Ok(data);
        }

        [Authorize(Roles = $"{UsersRoles.Management}, {UsersRoles.Organization}")]
        [HttpPost]
        [ProducesResponseType(typeof(VolunteerProgramGalleryPhotoDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> Update(
           [FromBody] UpdateGalleryPhotoDto updateEntityDto)
        {
            var userId = ParsingHelpers.ParseUserId(
                HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == null)
            {
                return Unauthorized();
            }

            VolunteerProgramGalleryPhotoDto data =
                await _volunteerProgramGalleryPhoto.Update(
                updateEntityDto, (long)userId);

            return Ok(data);
        }

        [Authorize(Roles = UsersRoles.Student)]
        [HttpPost]
        [ProducesResponseType(typeof(VolunteerProgramGalleryPhotoDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> UpdateByStudent(
            [FromBody] UpdateGalleryPhotoByStudentDto updateEntityDto)
        {
            var userId = ParsingHelpers.ParseUserId(
                HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == null)
            {
                return Unauthorized();
            }

            VolunteerProgramGalleryPhotoDto data =
                await _volunteerProgramGalleryPhoto.UpdateGalleryPhotoByStudent(
                updateEntityDto, (long)userId);

            return Ok(data);
        }

        [Authorize(Roles = $"{UsersRoles.Management}, {UsersRoles.Organization}")]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(VolunteerProgramGalleryPhotoDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]

        public async Task<IActionResult> Delete(long id)
        {
            VolunteerProgramGalleryPhotoDto data =
                await _volunteerProgramGalleryPhoto.Remove(id);

            return Ok(data);
        }
    }
}
