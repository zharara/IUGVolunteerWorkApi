using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Data;
using VolunteerWorkApi.Dtos.VolunteerProgramActivity;
using VolunteerWorkApi.Extensions;
using VolunteerWorkApi.Helpers.ErrorHandling;
using VolunteerWorkApi.Models;
using VolunteerWorkApi.Services.SavedFiles;

namespace VolunteerWorkApi.Services.VolunteerProgramActivities
{
    public class VolunteerProgramActivityService : IVolunteerProgramActivityService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ISavedFileService _savedFileService;

        public VolunteerProgramActivityService(
            ApplicationDbContext dbContext, IMapper mapper,
            ISavedFileService savedFileService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _savedFileService = savedFileService;
        }

        public IEnumerable<VolunteerProgramActivityDto> GetAll()
        {
            return _dbContext.VolunteerProgramActivities
                .Select(x => _mapper.Map<VolunteerProgramActivityDto>(x))
                .ToList();
        }

        public IEnumerable<VolunteerProgramActivityDto> GetList(string? filter,
            int? skipCount, int? maxResultCount, long? volunteerProgramId)
        {
            return _dbContext.VolunteerProgramActivities
                 .WhereIf(volunteerProgramId != null,
                    x => x.VolunteerProgramId == volunteerProgramId)
                 .WhereIf(!string.IsNullOrEmpty(filter),
                    x => x.Title.Contains(filter!))
                 .Skip(skipCount ?? 0)
                 .Take(maxResultCount ?? ApiConstants.MaxResultCount)
                 .Select(x => _mapper.Map<VolunteerProgramActivityDto>(x))
                 .ToList();
        }

        public VolunteerProgramActivityDto GetById(long id)
        {
            var data = _dbContext.VolunteerProgramActivities.Find(id);

            if (data == null)
            {
                throw new ApiNotFoundException();
            }

            return _mapper.Map<VolunteerProgramActivityDto>(data);
        }

        public async Task<VolunteerProgramActivityDto> Create(
            CreateVolunteerProgramActivityDto createEntityDto, long currentUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var entity = _mapper.Map<VolunteerProgramActivity>(createEntityDto);

                if (!createEntityDto.SaveTempFiles.IsNullOrEmpty())
                {
                    List<SavedFile> savedFiles = new();

                    foreach (SaveTempFile saveTempFile in
                        createEntityDto.SaveTempFiles)
                    {

                        var savedFile = await _savedFileService
                             .Create(saveTempFile);

                        savedFiles.Add(savedFile);
                    }

                    entity.Photos = savedFiles;
                }

                entity.CreatedBy = currentUserId;

                await _dbContext.VolunteerProgramActivities.AddAsync(entity);

                int effectedRows = await _dbContext.SaveChangesAsync();

                if (!(effectedRows > 0))
                {
                    throw new ApiDataException();
                }

                transaction.Commit();

                return _mapper.Map<VolunteerProgramActivityDto>(entity);
            }
            catch
            {
                transaction.Rollback();

                throw;
            }
        }

        public async Task<VolunteerProgramActivityDto> Update(
            UpdateVolunteerProgramActivityDto updateEntityDto,
            long currentUserId)
        {
            var entity = _dbContext.VolunteerProgramActivities.Find(updateEntityDto.Id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                entity = _mapper.Map(updateEntityDto, entity);

                if (!updateEntityDto.SaveTempFiles.IsNullOrEmpty())
                {
                    List<SavedFile> savedFiles = new();

                    foreach (SaveTempFile saveTempFile in
                        updateEntityDto.SaveTempFiles!)
                    {

                        var savedFile = await _savedFileService
                             .Create(saveTempFile);

                        savedFiles.Add(savedFile);
                    }

                    entity.Photos = savedFiles;
                }

                entity.ModifiedDate = DateTime.UtcNow;

                entity.ModifiedBy = currentUserId;

                _dbContext.VolunteerProgramActivities.Update(entity);

                int effectedRows = await _dbContext.SaveChangesAsync();

                if (!(effectedRows > 0))
                {
                    throw new ApiDataException();
                }

                transaction.Commit();

                return _mapper.Map<VolunteerProgramActivityDto>(entity);
            }
            catch
            {
                transaction.Rollback();

                throw;
            }
        }

        public async Task<VolunteerProgramActivityDto> Remove(long id)
        {
            var entity = _dbContext.VolunteerProgramActivities.Find(id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity.IsDeleted = true;

            _dbContext.VolunteerProgramActivities.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<VolunteerProgramActivityDto>(entity);
        }
    }
}
