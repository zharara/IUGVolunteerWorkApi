using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VolunteerWorkApi.Dtos.SavedFile;
using VolunteerWorkApi.Models;
using VolunteerWorkApi.Services.SavedFiles;

namespace VolunteerWorkApi.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SavedFilesController : ControllerBase
    {
        private readonly ISavedFileService _savedFileService;

        public SavedFilesController(
            ISavedFileService savedFileService)
        {
            _savedFileService = savedFileService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SavedFileDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]

        public IActionResult Get(long id)
        {
            SavedFileDto data =
                _savedFileService.GetById(id: id);

            return Ok(data);
        }
    }
}
