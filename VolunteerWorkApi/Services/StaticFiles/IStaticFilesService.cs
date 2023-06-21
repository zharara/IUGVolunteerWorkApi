
namespace VolunteerWorkApi.Services.StaticFiles
{
    public interface IStaticFilesService
    {
        string PutTempFile(IFormFile formFile);

        FileInfo MoveToSavedFiles(string tempFileKey);

        void DeleteSavedFile(string savedFileKey);
    }
}
