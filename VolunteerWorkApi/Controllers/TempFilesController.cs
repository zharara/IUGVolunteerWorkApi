using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Dtos.TempFile;
using VolunteerWorkApi.Helpers.ErrorHandling;
using VolunteerWorkApi.Models;
using VolunteerWorkApi.Services.TempFiles;

namespace VolunteerWorkApi.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TempFilesController : ControllerBase
    {
        private readonly ITempFileService _tempFileService;

        public TempFilesController(
            ITempFileService tempFileService)
        {
            _tempFileService = tempFileService;
        }

        [HttpPost, DisableRequestSizeLimit]
        [ProducesResponseType(typeof(TempFileDto),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> UploadOneFile()
        {
            if (Request.HasFormContentType)
            {
                // Only one file exactly
                if (Request.Form.Files.Count != 1)
                {
                    throw new ApiResponseException(
                        HttpStatusCode.BadRequest,
                        ErrorMessages.DataError,
                        ErrorMessages.OnlyOneFileAllowedToUpload);
                }

                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files[0];

                if (file.Length > 0)
                {
                    TempFileDto tempFileDto = await _tempFileService.Create(file);

                    return Ok(tempFileDto);
                }
                else
                {
                    throw new ApiResponseException(
                        HttpStatusCode.BadRequest,
                         ErrorMessages.DataError,
                         ErrorMessages.AFileUploadedIsCorrupt);
                }
            }
            else
            {
                throw new ApiResponseException(
                    HttpStatusCode.BadRequest,
                    ErrorMessages.DataError,
                    ErrorMessages.MustUploadAtLeastOneFile);
            }
        }

        [HttpPost, DisableRequestSizeLimit]
        [ProducesResponseType(typeof(IEnumerable<TempFileDto>),
            (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> UploadFiles()
        {
            if (Request.HasFormContentType)
            {
                // One or more only
                if (Request.Form.Files.Count < 1)
                {
                    throw new ApiResponseException(
                        HttpStatusCode.BadRequest,
                        ErrorMessages.DataError,
                        ErrorMessages.MustUploadAtLeastOneFile);
                }

                var formCollection = await Request.ReadFormAsync();
                var files = formCollection.Files;

                if (files.Any(f => f.Length == 0))
                {
                    throw new ApiResponseException(
                        HttpStatusCode.BadRequest,
                        ErrorMessages.DataError,
                        ErrorMessages.AFileUploadedIsCorrupt);
                }

                List<TempFileDto> tempFiles = new();

                foreach (var file in files)
                {
                    TempFileDto tempFileDto = await _tempFileService.Create(file);

                    tempFiles.Add(tempFileDto);
                }

                return Ok(tempFiles);
            }
            else
            {
                throw new ApiResponseException(
                    HttpStatusCode.BadRequest,
                    ErrorMessages.DataError,
                    ErrorMessages.MustUploadAtLeastOneFile);
            }
        }
    }
}
