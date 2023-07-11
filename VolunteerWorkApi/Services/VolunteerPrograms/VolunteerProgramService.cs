using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VolunteerWorkApi.Data;
using VolunteerWorkApi.Dtos.VolunteerProgram;
using VolunteerWorkApi.Extensions;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Helpers.ErrorHandling;
using VolunteerWorkApi.Services.SavedFiles;
using System.Net;
using VolunteerWorkApi.Services.StudentApplications;
using VolunteerWorkApi.Services.VolunteerStudents;
using VolunteerWorkApi.Dtos.StudentApplication;
using VolunteerWorkApi.Dtos.VolunteerStudent;
using VolunteerWorkApi.Enums;

namespace VolunteerWorkApi.Services.VolunteerPrograms
{
    public class VolunteerProgramService : IVolunteerProgramService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ISavedFileService _savedFileService;
        private readonly IStudentApplicationService _studentApplicationService;
        private readonly IVolunteerStudentService _volunteerStudentService;

        public VolunteerProgramService(
            ApplicationDbContext dbContext, IMapper mapper,
            ISavedFileService savedFileService,
            IStudentApplicationService studentApplicationService,
            IVolunteerStudentService volunteerStudentService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _savedFileService = savedFileService;
            _studentApplicationService = studentApplicationService;
            _volunteerStudentService = volunteerStudentService;
        }

        public IEnumerable<VolunteerProgramDto> GetAll()
        {
            return _dbContext.VolunteerPrograms
                .Include(x => x.Organization)
                 .ThenInclude(x => x.ProfilePicture)
                 .Include(x => x.Category)
                 .Include(x => x.Logo)
                .Select(x => _mapper.Map<VolunteerProgramDto>(x))
                .ToList();
        }

        public IEnumerable<VolunteerProgramDto> GetList(string? filter,
            int? skipCount, int? maxResultCount,
            long? organizationId, bool? isInternalProgram)
        {
            return _dbContext.VolunteerPrograms
                 .Include(x => x.Organization)
                 .ThenInclude(x => x.ProfilePicture)
                 .Include(x => x.Category)
                 .Include(x => x.Logo)
                 .WhereIf(isInternalProgram != null,
                    x => x.IsInternalProgram == isInternalProgram)
                 .WhereIf(organizationId != null,
                    x => x.OrganizationId == organizationId)
                 .WhereIf(!string.IsNullOrEmpty(filter),
                    x => x.Title.Contains(filter!))
                 .Skip(skipCount ?? 0)
                 .Take(maxResultCount ?? ApiConstants.MaxResultCount)
                 .Select(x => _mapper.Map<VolunteerProgramDto>(x))
                 .ToList();
        }

        public VolunteerProgramDto GetById(long id)
        {
            var data = _dbContext.VolunteerPrograms.Find(id);

            if (data == null)
            {
                throw new ApiNotFoundException();
            }

            return _mapper.Map<VolunteerProgramDto>(data);
        }

        public async Task<VolunteerProgramDto> Create(
            CreateVolunteerProgramDto createEntityDto, long currentUserId)
        {
            if (createEntityDto.CategoryId == null
                && createEntityDto.Category is null)
            {
                throw new ApiResponseException(
                    HttpStatusCode.BadRequest,
                    ErrorMessages.InputError,
                    ErrorMessages.RequiredFieldsNotInputted,
                    nameof(createEntityDto.Category));
            }

            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var entity = _mapper.Map<VolunteerProgram>(createEntityDto);

                if (createEntityDto.SaveTempFile != null)
                {
                    var savedFile = await _savedFileService
                         .Create(createEntityDto.SaveTempFile);

                    entity.Logo = savedFile;
                }

                entity.IsInternalProgram = false;

                entity.CreatedBy = currentUserId;

                var addedEntity = await _dbContext.VolunteerPrograms.AddAsync(entity);

                int effectedRows = await _dbContext.SaveChangesAsync();

                if (!(effectedRows > 0))
                {
                    throw new ApiDataException();
                }

                if (createEntityDto.VolunteerOpportunityId != null)
                {
                    var studentsApplications = _studentApplicationService
                           .GetListOfOpportunity(
                            (long)createEntityDto.VolunteerOpportunityId);

                    foreach (StudentApplication applicationDto in studentsApplications)
                    {
                        if (applicationDto.StatusForManagement == ApplicationStatus.Approved)
                        {
                            await _volunteerStudentService.CreateEntity(
                                studentId: applicationDto.StudentId,
                                volunteerProgramId: entity.Id);
                        }
                    }
                }

                var item = _dbContext.VolunteerPrograms
                          .Include(x => x.Organization)
                         .ThenInclude(x => x.ProfilePicture)
                         .Include(x => x.Category)
                         .Include(x => x.Logo)
                          .Where(x => x.Id == addedEntity.Entity.Id)
                          .FirstOrDefault();

                transaction.Commit();

                return _mapper.Map<VolunteerProgramDto>(item);
            }
            catch
            {
                transaction.Rollback();

                throw;
            }
        }

        public async Task<VolunteerProgramDto> CreateInternalProgram(
            CreateInternalVolunteerProgramDto createEntityDto, long currentUserId)
        {
            if (createEntityDto.CategoryId == null
                && createEntityDto.Category is null)
            {
                throw new ApiResponseException(
                     HttpStatusCode.BadRequest,
                    ErrorMessages.InputError,
                    ErrorMessages.RequiredFieldsNotInputted,
                    nameof(createEntityDto.Category));
            }

            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var entity = _mapper.Map<VolunteerProgram>(createEntityDto);

                if (createEntityDto.SaveTempFile != null)
                {
                    var savedFile = await _savedFileService
                         .Create(createEntityDto.SaveTempFile);

                    entity.Logo = savedFile;
                }

                entity.IsInternalProgram = true;

                entity.CreatedBy = currentUserId;

                var addedEntity = await _dbContext.VolunteerPrograms.AddAsync(entity);

                int effectedRows = await _dbContext.SaveChangesAsync();

                if (!(effectedRows > 0))
                {
                    throw new ApiDataException();
                }

                var item = _dbContext.VolunteerPrograms
                        .Include(x => x.Organization)
                       .ThenInclude(x => x.ProfilePicture)
                       .Include(x => x.Category)
                       .Include(x => x.Logo)
                        .Where(x => x.Id == addedEntity.Entity.Id)
                        .FirstOrDefault();

                transaction.Commit();

                return _mapper.Map<VolunteerProgramDto>(item);
            }
            catch
            {
                transaction.Rollback();

                throw;
            }
        }

        public async Task<VolunteerProgramDto> Update(
            UpdateVolunteerProgramDto updateEntityDto, long currentUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var entity = _dbContext.VolunteerPrograms
                        .Include(x => x.Organization)
                       .ThenInclude(x => x.ProfilePicture)
                       .Include(x => x.Category)
                       .Include(x => x.Logo)
                        .Where(x => x.Id == updateEntityDto.Id)
                        .FirstOrDefault();

                if (entity == null)
                {
                    throw new ApiNotFoundException();
                }

                entity = _mapper.Map(updateEntityDto, entity);

                if (updateEntityDto.SaveTempFile != null)
                {
                    var savedFile = await _savedFileService
                         .Create(updateEntityDto.SaveTempFile);

                    entity.Logo = savedFile;
                }

                entity.ModifiedDate = DateTime.UtcNow;

                entity.ModifiedBy = currentUserId;

                _dbContext.VolunteerPrograms.Update(entity);

                int effectedRows = await _dbContext.SaveChangesAsync();

                if (!(effectedRows > 0))
                {
                    throw new ApiDataException();
                }

                transaction.Commit();

                return _mapper.Map<VolunteerProgramDto>(entity);
            }
            catch
            {
                transaction.Rollback();

                throw;
            }
        }

        public async Task<VolunteerProgramDto> UpdateInternalProgram(
            UpdateInternalVolunteerProgramDto updateEntityDto, long currentUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var entity = _dbContext.VolunteerPrograms
                            .Include(x => x.Organization)
                           .ThenInclude(x => x.ProfilePicture)
                           .Include(x => x.Category)
                           .Include(x => x.Logo)
                            .Where(x => x.Id == updateEntityDto.Id)
                            .FirstOrDefault();

                if (entity == null)
                {
                    throw new ApiNotFoundException();
                }

                entity = _mapper.Map(updateEntityDto, entity);

                if (updateEntityDto.SaveTempFile != null)
                {
                    var savedFile = await _savedFileService
                         .Create(updateEntityDto.SaveTempFile);

                    entity.Logo = savedFile;
                }

                entity.ModifiedDate = DateTime.UtcNow;

                entity.ModifiedBy = currentUserId;

                _dbContext.VolunteerPrograms.Update(entity);

                int effectedRows = await _dbContext.SaveChangesAsync();

                if (!(effectedRows > 0))
                {
                    throw new ApiDataException();
                }

                transaction.Commit();

                return _mapper.Map<VolunteerProgramDto>(entity);
            }
            catch
            {
                transaction.Rollback();

                throw;
            }
        }

        public async Task<VolunteerProgramDto> Remove(long id)
        {
            var entity = _dbContext.VolunteerPrograms.Find(id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity.IsDeleted = true;

            _dbContext.VolunteerPrograms.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<VolunteerProgramDto>(entity);
        }
    }
}
