using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Helpers.ErrorHandling;

namespace VolunteerWorkApi.Services.StaticFiles
{
    public class StaticFilesService : IStaticFilesService
    {
        public string PutTempFile(IFormFile formFile)
        {
            try
            {
                var folderName = Path.Combine(
                    FoldersNames.UsersFiles, FoldersNames.TempFiles);

                var pathToSave = Path.Combine(
                    Directory.GetCurrentDirectory(), folderName);

                if (!Directory.Exists(pathToSave))
                    Directory.CreateDirectory(pathToSave);

                var fileKey = Guid.NewGuid().ToString()
                    + Path.GetExtension(formFile.FileName);

                var fullPath = Path.Combine(pathToSave, fileKey);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    formFile.CopyTo(stream);
                }

                return fileKey;
            }
            catch
            {
                throw new ApiDataException();
            }
        }

        public FileInfo MoveToSavedFiles(string tempFileKey)
        {
            try
            {
                var sourceFolder = Path.Combine(
                    FoldersNames.UsersFiles, FoldersNames.TempFiles);

                var pathToSource = Path.Combine(
                    Directory.GetCurrentDirectory(), sourceFolder);

                var sourceFile = Path.Combine(pathToSource, tempFileKey);

                var destinationFolder = Path.Combine(
                    FoldersNames.UsersFiles, FoldersNames.SavedFiles);
                var pathToDestination = Path.Combine(
                    Directory.GetCurrentDirectory(), destinationFolder);

                if (!Directory.Exists(pathToDestination))
                    Directory.CreateDirectory(pathToDestination);

                string destinationFile = Path.Combine(
                    pathToDestination, tempFileKey);

                File.Move(sourceFile, destinationFile);

                return new FileInfo(destinationFile);
            }
            catch
            {
                throw new ApiDataException();
            }
        }

        public void DeleteSavedFile(string savedFileKey)
        {
            try
            {
                var savedFilesFolder = Path.Combine(
                    FoldersNames.UsersFiles, FoldersNames.SavedFiles);
                var pathToSavedFiles = Path.Combine(
                    Directory.GetCurrentDirectory(), savedFilesFolder);

                string savedFilePath = Path.Combine(
                    pathToSavedFiles, savedFileKey);

                File.Delete(savedFilePath);
            }
            catch
            {
                throw new ApiDataException();
            }
        }
    }
}
