using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Data;
using VolunteerWorkApi.Dtos.VolunteerStudent;
using VolunteerWorkApi.Extensions;
using VolunteerWorkApi.Helpers.ErrorHandling;

namespace VolunteerWorkApi.Services.VolunteerStudents
{
    public class VolunteerStudentService : IVolunteerStudentService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public VolunteerStudentService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IEnumerable<VolunteerStudentDto> GetAll()
        {
            return _dbContext.VolunteerStudents
                .Include(x => x.Student)
                .Include(x => x.VolunteerProgram)
                .ThenInclude(x => x.Organization)
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

        public async Task<VolunteerStudentDto> Create(
            CreateVolunteerStudentDto createEntityDto, long currentUserId)
        {
            var entity = _mapper.Map<VolunteerStudent>(createEntityDto);

            entity.CreatedBy = currentUserId;

            await _dbContext.VolunteerStudents.AddAsync(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<VolunteerStudentDto>(entity);
        }

        public async Task<VolunteerStudent> CreateEntity(
            long studentId, long volunteerProgramId)
        {
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

            return entity;
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
