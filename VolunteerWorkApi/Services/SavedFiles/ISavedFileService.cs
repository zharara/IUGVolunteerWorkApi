using VolunteerWorkApi.Dtos.SavedFile;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Services.SavedFiles
{
    public interface ISavedFileService
    {
        SavedFileDto GetById(long id);

        SavedFile GetEntityById(long id);

        Task<SavedFile> Create(SaveTempFile saveTempFile);

        Task<SavedFile> Remove(long id);
    }
}
