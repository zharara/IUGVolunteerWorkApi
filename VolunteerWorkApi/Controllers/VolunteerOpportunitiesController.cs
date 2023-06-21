using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Dtos.VolunteerOpportunity;
using VolunteerWorkApi.Helpers;
using VolunteerWorkApi.Models;
using VolunteerWorkApi.Services.VolunteerOpportunities;

namespace VolunteerWorkApi.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VolunteerOpportunitiesController : ControllerBase
    {
        private readonly IVolunteerOpportunityService _volunteerOpportunityService;

        public VolunteerOpportunitiesController(
            IVolunteerOpportunityService volunteerOpportunityService)
        {
            _volunteerOpportunityService = volunteerOpportunityService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<VolunteerOpportunityDto>),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public IActionResult GetAll()
        {
            IEnumerable<VolunteerOpportunityDto> data =
                _volunteerOpportunityService.GetAll();

            return Ok(data);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<VolunteerOpportunityDto>),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public IActionResult GetList(
            string? filter, int? skipCount,
            int? maxResultCount, long? organizationId)
        {
            IEnumerable<VolunteerOpportunityDto> data =
                _volunteerOpportunityService
                .GetList(filter: filter, skipCount: skipCount,
                maxResultCount: maxResultCount, organizationId: organizationId);

            return Ok(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(VolunteerOpportunityDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]

        public IActionResult Get(long id)
        {
            VolunteerOpportunityDto data =
                _volunteerOpportunityService.GetById(id: id);

            return Ok(data);
        }

        [Authorize(Roles = UsersRoles.Organization)]
        [HttpPost]
        [ProducesResponseType(typeof(VolunteerOpportunityDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> Create(
            [FromBody] CreateVolunteerOpportunityDto createEntityDto)
        {
            var userId = ParsingHelpers.ParseUserId(
                HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == null)
            {
                return Unauthorized();
            }


            VolunteerOpportunityDto data =
                await _volunteerOpportunityService
                .Create(createEntityDto, (long)userId);

            return Ok(data);
        }

        [Authorize(Roles = UsersRoles.Organization)]
        [HttpPut]
        [ProducesResponseType(typeof(VolunteerOpportunityDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]

        public async Task<IActionResult> Update(
           [FromBody] UpdateVolunteerOpportunityDto updateEntityDto)
        {
            var userId = ParsingHelpers.ParseUserId(
                HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == null)
            {
                return Unauthorized();
            }

            VolunteerOpportunityDto data =
                await _volunteerOpportunityService.Update(
                updateEntityDto, (long)userId);

            return Ok(data);
        }

        [Authorize(Roles = UsersRoles.Organization)]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(VolunteerOpportunityDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]

        public async Task<IActionResult> Delete(long id)
        {
            VolunteerOpportunityDto data =
                await _volunteerOpportunityService.Remove(id);

            return Ok(data);
        }
    }
}
