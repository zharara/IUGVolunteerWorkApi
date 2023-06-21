using AutoMapper;
using VolunteerWorkApi.Data;
using VolunteerWorkApi.Dtos.SavedFile;
using VolunteerWorkApi.Helpers.ErrorHandling;
using VolunteerWorkApi.Models;
using VolunteerWorkApi.Services.StaticFiles;
using VolunteerWorkApi.Services.TempFiles;

namespace VolunteerWorkApi.Services.SavedFiles
{
    public class SavedFileService : ISavedFileService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ITempFileService _tempFileService;
        private readonly IStaticFilesService _staticFilesService;

        public SavedFileService(
            ApplicationDbContext dbContext,
            IMapper mapper, ITempFileService tempFileService,
            IStaticFilesService staticFilesService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _tempFileService = tempFileService;
            _staticFilesService = staticFilesService;
        }

        public SavedFileDto GetById(long id)
        {
            var data = _dbContext.SavedFiles.Find(id);

            if (data == null)
            {
                throw new ApiNotFoundException();
            }

            return _mapper.Map<SavedFileDto>(data);
        }

        public SavedFile GetEntityById(long id)
        {
            var data = _dbContext.SavedFiles.Find(id);

            if (data == null)
            {
                throw new ApiNotFoundException();
            }

            return data;
        }

        public async Task<SavedFile> Create(
            SaveTempFile saveTempFile)
        {
            var tempFile = _tempFileService
            .GetEntityById(saveTempFile.TempFileId);

            // Move the physical file
            var fileInfo = _staticFilesService.MoveToSavedFiles(tempFile.FileKey);

            var entity = new SavedFile
            {
                FileKey = tempFile.FileKey,
                OriginalFileName = tempFile.OriginalFileName,
                FileSize = fileInfo.Length,
                FileExtension = fileInfo.Extension,
            };

            await _dbContext.SavedFiles.AddAsync(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            // Remove the record
            await _tempFileService.Remove(tempFile.Id);

            return entity;
        }

        public async Task<SavedFile> Remove(long id)
        {
            var entity = _dbContext.SavedFiles.Find(id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            _staticFilesService.DeleteSavedFile(entity.FileKey);

            _dbContext.SavedFiles.Remove(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return entity;
        }
    }
}
