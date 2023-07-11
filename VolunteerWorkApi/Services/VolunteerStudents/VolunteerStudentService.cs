using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Data;
using VolunteerWorkApi.Dtos.VolunteerStudent;
using VolunteerWorkApi.Extensions;
using VolunteerWorkApi.Helpers.ErrorHandling;
using VolunteerWorkApi.Services.Students;

namespace VolunteerWorkApi.Services.VolunteerStudents
{
    public class VolunteerStudentService : IVolunteerStudentService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IStudentService _studentService;

        public VolunteerStudentService(ApplicationDbContext dbContext,
            IMapper mapper, IStudentService studentService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _studentService = studentService;
        }

        public IEnumerable<VolunteerStudentDto> GetAll()
        {
            return _dbContext.VolunteerStudents
                .Include(x => x.Student)
                .Include(x => x.VolunteerProgram)
                .ThenInclude(x => x.Organization)
                .ThenInclude(x => x.ProfilePicture)
                .Include(x => x.VolunteerProgram)
                .ThenInclude(x => x.Category)
                .Select(x => _mapper.Map<VolunteerStudentDto>(x))
                .ToList();
        }

        public IEnumerable<VolunteerStudentDto> GetList(string? filter,
            int? skipCount, int? maxResultCount, long? volunteerProgramId,
            long? organizationId)
        {
            return _dbContext.VolunteerStudents
                 .Include(x => x.Student)
                 .Include(x => x.VolunteerProgram)
                 .ThenInclude(x => x.Organization)
                 .ThenInclude(x => x.ProfilePicture)
                 .Include(x => x.VolunteerProgram)
                 .ThenInclude(x => x.Category)
                 .WhereIf(volunteerProgramId != null,
                    x => x.VolunteerProgramId == volunteerProgramId)
                 .WhereIf(!string.IsNullOrEmpty(filter),
                    x => x.Student.FullName.Contains(filter!))
                 .WhereIf(organizationId != null,
                   x => x.VolunteerProgram.OrganizationId == organizationId)
                 .Skip(skipCount ?? 0)
                 .Take(maxResultCount ?? ApiConstants.MaxResultCount)
                 .Select(x => _mapper.Map<VolunteerStudentDto>(x))
                 .ToList();
        }

        public VolunteerStudentDto GetById(long id)
        {
            var data = _dbContext.VolunteerStudents.Find(id);

            if (data == null)
            {
                throw new ApiNotFoundException();
            }

            return _mapper.Map<VolunteerStudentDto>(data);
        }

        public VolunteerStudentDto? GetOfStudentById(long studentId)
        {
            var data = _dbContext.VolunteerStudents
                .Include(x => x.Student)
                .Include(x => x.VolunteerProgram)
                .ThenInclude(x => x.Organization)
                .ThenInclude(x => x.ProfilePicture)
                .Include(x => x.VolunteerProgram)
                .ThenInclude(x => x.Category)
                .Where(x => x.StudentId == studentId)
                .Select(x => _mapper.Map<VolunteerStudentDto>(x))
                .FirstOrDefault();

            if (data == null)
            {
                return null;
            }

            return data;
        }

        public async Task<VolunteerStudentDto> Create(
            CreateVolunteerStudentDto createEntityDto, long currentUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var existingVolunteerStudent = _dbContext.VolunteerStudents.FirstOrDefault(
                     x => x.StudentId == createEntityDto.StudentId);

                if (existingVolunteerStudent != null)
                {
                    throw new ApiResponseException(
                      HttpStatusCode.BadRequest,
                      ErrorMessages.ConflictError,
                      ErrorMessages.StudentAlreayEnrollerInProgram,
                      existingVolunteerStudent.VolunteerProgram.Title);
                }

                var entity = _mapper.Map<VolunteerStudent>(createEntityDto);

                entity.CreatedBy = currentUserId;

               var addedEntity = await _dbContext.VolunteerStudents.AddAsync(entity);

                int effectedRows = await _dbContext.SaveChangesAsync();

                if (!(effectedRows > 0))
                {
                    throw new ApiDataException();
                }

                await _studentService.StudentHasEnrolledInProgram(createEntityDto.StudentId);

                var item = _dbContext.VolunteerStudents
                .Include(x => x.Student)
                .Include(x => x.VolunteerProgram)
                .ThenInclude(x => x.Organization)
                .ThenInclude(x => x.ProfilePicture)
                .Include(x => x.VolunteerProgram)
                .ThenInclude(x => x.Category)
                .Where(x => x.StudentId == addedEntity.Entity.Id)
                .Select(x => _mapper.Map<VolunteerStudentDto>(x))
                .FirstOrDefault();

                transaction.Commit();

                return _mapper.Map<VolunteerStudentDto>(item);
            }
            catch
            {
                transaction.Rollback();

                throw;
            }
        }

        public async Task<VolunteerStudent> CreateEntity(
            long studentId, long volunteerProgramId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var existingVolunteerStudent = _dbContext.VolunteerStudents.FirstOrDefault(
                    x => x.StudentId == studentId);

                if (existingVolunteerStudent != null)
                {
                    throw new ApiResponseException(
                      HttpStatusCode.BadRequest,
                      ErrorMessages.ConflictError,
                      ErrorMessages.StudentAlreayEnrollerInProgram,
                      existingVolunteerStudent.VolunteerProgram.Title);
                }

                var entity = new VolunteerStudent
                {
                    StudentId = studentId,
                    VolunteerProgramId = volunteerProgramId
                };

                await _dbContext.VolunteerStudents.AddAsync(entity);

                int effectedRows = await _dbContext.SaveChangesAsync();

                if (!(effectedRows > 0))
                {
                    throw new ApiDataException();
                }

                await _studentService.StudentHasEnrolledInProgram(studentId);

                transaction.Commit();

                return entity;
            }
            catch
            {
                transaction.Rollback();

                throw;
            }
        }

        public async Task<VolunteerStudentDto> UpdateOrganizationAssessment(
            UpdateVolunteerStudentOrgAssessmentDto updateEntityDto,
            long currentUserId)
        {
            var entity = _dbContext.VolunteerStudents.Find(updateEntityDto.Id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity = _mapper.Map(updateEntityDto, entity);

            entity.ModifiedDate = DateTime.UtcNow;

            entity.ModifiedBy = currentUserId;

            _dbContext.VolunteerStudents.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<VolunteerStudentDto>(entity);
        }

        public async Task<VolunteerStudentDto> UpdateManagementFinalEvaluation(
            UpdateVolunteerStudentFinalEvaluationDto updateEntityDto,
            long currentUserId)
        {
            var entity = _dbContext.VolunteerStudents.Find(updateEntityDto.Id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity = _mapper.Map(updateEntityDto, entity);

            entity.ModifiedDate = DateTime.UtcNow;

            entity.ModifiedBy = currentUserId;

            _dbContext.VolunteerStudents.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<VolunteerStudentDto>(entity);
        }

        public async Task<VolunteerStudentDto> Remove(long id)
        {
            var entity = _dbContext.VolunteerStudents.Find(id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity.IsDeleted = true;

            _dbContext.VolunteerStudents.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<VolunteerStudentDto>(entity);
        }
    }
}
