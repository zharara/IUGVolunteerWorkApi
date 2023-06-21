using AutoMapper;
using VolunteerWorkApi.Data;
using VolunteerWorkApi.Dtos.TempFile;
using VolunteerWorkApi.Helpers.ErrorHandling;
using VolunteerWorkApi.Services.StaticFiles;

namespace VolunteerWorkApi.Services.TempFiles
{
    public class TempFileService : ITempFileService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IStaticFilesService _staticFilesService;

        public TempFileService(
            ApplicationDbContext dbContext,
            IMapper mapper,
            IStaticFilesService staticFilesService)
        {
            _dbContext = dbContext;
            _staticFilesService = staticFilesService;
            _mapper = mapper;
        }

        public TempFile GetEntityById(long id)
        {
            var data = _dbContext.TempFiles.Find(id);

            if (data == null)
            {
                throw new ApiNotFoundException();
            }

            return data;
        }

        public async Task<TempFileDto> Create(
            IFormFile formFile)
        {
            string tempFileKey = _staticFilesService.PutTempFile(formFile);

            var entity = new TempFile
            {
                FileKey = tempFileKey,
                OriginalFileName = formFile.FileName,
            };

            await _dbContext.TempFiles.AddAsync(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<TempFileDto>(entity);
        }

        public async Task<TempFile> Remove(long id)
        {
            var entity = _dbContext.TempFiles.Find(id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            _dbContext.TempFiles.Remove(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return entity;
        }
    }
}
