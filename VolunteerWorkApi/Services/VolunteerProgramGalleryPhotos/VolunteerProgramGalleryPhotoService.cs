using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Data;
using VolunteerWorkApi.Dtos.VolunteerProgramGalleryPhoto;
using VolunteerWorkApi.Extensions;
using VolunteerWorkApi.Helpers.ErrorHandling;
using VolunteerWorkApi.Services.SavedFiles;

namespace VolunteerWorkApi.Services.VolunteerProgramGalleryPhotos
{
    public class VolunteerProgramGalleryPhotoService
        : IVolunteerProgramGalleryPhotoService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ISavedFileService _savedFileService;

        public VolunteerProgramGalleryPhotoService(
            ApplicationDbContext dbContext, IMapper mapper,
            ISavedFileService savedFileService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _savedFileService = savedFileService;
        }

        public IEnumerable<VolunteerProgramGalleryPhotoDto> GetAll()
        {
            return _dbContext.VolunteerProgramGalleryPhotos
                .Include(x => x.Photo)
                .Select(x => _mapper.Map<VolunteerProgramGalleryPhotoDto>(x))
                .ToList();
        }

        public IEnumerable<VolunteerProgramGalleryPhotoDto> GetList(
            int? skipCount, int? maxResultCount,
            long? volunteerProgramId, bool? isApproved)
        {
            return _dbContext.VolunteerProgramGalleryPhotos
                 .Include(x => x.Photo)
                 .WhereIf(volunteerProgramId != null,
                    x => x.VolunteerProgramId == volunteerProgramId)
                 .WhereIf(isApproved != null,
                    x => x.IsApproved == isApproved)
                 .Skip(skipCount ?? 0)
                 .Take(maxResultCount ?? ApiConstants.MaxResultCount)
                 .Select(x => _mapper.Map<VolunteerProgramGalleryPhotoDto>(x))
                 .ToList();
        }

        public VolunteerProgramGalleryPhotoDto GetById(long id)
        {
            var data = _dbContext.VolunteerProgramGalleryPhotos.Find(id);

            if (data == null)
            {
                throw new ApiNotFoundException();
            }

            return _mapper.Map<VolunteerProgramGalleryPhotoDto>(data);
        }

        public async Task<VolunteerProgramGalleryPhotoDto> Create(
            CreateGalleryPhotoDto createEntityDto,
            long currentUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var entity = _mapper.Map<VolunteerProgramGalleryPhoto>(createEntityDto);

                var savedFile = await _savedFileService
                    .Create(createEntityDto.SaveTempFile);

                entity.Photo = savedFile;

                entity.IsApproved = true;

                entity.CreatedBy = currentUserId;

                await _dbContext.VolunteerProgramGalleryPhotos.AddAsync(entity);

                int effectedRows = await _dbContext.SaveChangesAsync();

                if (!(effectedRows > 0))
                {
                    throw new ApiDataException();
                }

                transaction.Commit();

                return _mapper.Map<VolunteerProgramGalleryPhotoDto>(entity);
            }
            catch
            {
                transaction.Rollback();

                throw;
            }
        }

        public async Task<VolunteerProgramGalleryPhotoDto> CreateGalleryPhotoByStudent(
            CreateGalleryPhotoByStudentDto createEntityDto,
            long currentUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var entity = _mapper.Map<VolunteerProgramGalleryPhoto>(createEntityDto);

                var savedFile = await _savedFileService
                    .Create(createEntityDto.SaveTempFile);

                entity.Photo = savedFile;

                entity.IsApproved = false;

                entity.CreatedBy = currentUserId;

                await _dbContext.VolunteerProgramGalleryPhotos.AddAsync(entity);

                int effectedRows = await _dbContext.SaveChangesAsync();

                if (!(effectedRows > 0))
                {
                    throw new ApiDataException();
                }

                transaction.Commit();

                return _mapper.Map<VolunteerProgramGalleryPhotoDto>(entity);
            }
            catch
            {
                transaction.Rollback();

                throw;
            }
        }

        public async Task<VolunteerProgramGalleryPhotoDto> Update(
            UpdateGalleryPhotoDto updateEntityDto,
            long currentUserId)
        {
            var entity = _dbContext.VolunteerProgramGalleryPhotos
                .Find(updateEntityDto.Id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                entity = _mapper.Map(updateEntityDto, entity);

                if (updateEntityDto.SaveTempFile != null)
                {
                    var savedFile = await _savedFileService
                         .Create(updateEntityDto.SaveTempFile);

                    entity.Photo = savedFile;
                }

                entity.ModifiedDate = DateTime.UtcNow;

                entity.ModifiedBy = currentUserId;

                _dbContext.VolunteerProgramGalleryPhotos.Update(entity);

                int effectedRows = await _dbContext.SaveChangesAsync();

                if (!(effectedRows > 0))
                {
                    throw new ApiDataException();
                }

                transaction.Commit();

                return _mapper.Map<VolunteerProgramGalleryPhotoDto>(entity);
            }
            catch
            {
                transaction.Rollback();

                throw;
            }
        }

        public async Task<VolunteerProgramGalleryPhotoDto> UpdateGalleryPhotoByStudent(
            UpdateGalleryPhotoByStudentDto updateEntityDto,
            long currentUserId)
        {
            var entity = _dbContext.VolunteerProgramGalleryPhotos
                .Find(updateEntityDto.Id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            if (entity.VolunteerStudentUploaderId
                != updateEntityDto.VolunteerStudentUploaderId)
            {
                throw new ApiResponseException(
                    HttpStatusCode.Unauthorized,
                    ErrorMessages.PermissionsError,
                    ErrorMessages.NoPermissionsForAccount);
            }

            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                entity = _mapper.Map(updateEntityDto, entity);

                if (updateEntityDto.SaveTempFile != null)
                {
                    var savedFile = await _savedFileService
                         .Create(updateEntityDto.SaveTempFile);

                    entity.Photo = savedFile;
                }

                entity.ModifiedDate = DateTime.UtcNow;

                entity.ModifiedBy = currentUserId;

                _dbContext.VolunteerProgramGalleryPhotos.Update(entity);

                int effectedRows = await _dbContext.SaveChangesAsync();

                if (!(effectedRows > 0))
                {
                    throw new ApiDataException();
                }

                transaction.Commit();

                return _mapper.Map<VolunteerProgramGalleryPhotoDto>(entity);
            }
            catch
            {
                transaction.Rollback();

                throw;
            }
        }

        public async Task<VolunteerProgramGalleryPhotoDto> Remove(long id)
        {
            var entity = _dbContext.VolunteerProgramGalleryPhotos.Find(id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity.IsDeleted = true;

            _dbContext.VolunteerProgramGalleryPhotos.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<VolunteerProgramGalleryPhotoDto>(entity);
        }
    }
}
