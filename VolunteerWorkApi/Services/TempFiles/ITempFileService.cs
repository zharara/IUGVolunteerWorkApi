using VolunteerWorkApi.Dtos.TempFile;

namespace VolunteerWorkApi.Services.TempFiles
{
    public interface ITempFileService
    {
        TempFile GetEntityById(long id);

        Task<TempFileDto> Create(IFormFile formFile);

        Task<TempFile> Remove(long id);
    }
}
