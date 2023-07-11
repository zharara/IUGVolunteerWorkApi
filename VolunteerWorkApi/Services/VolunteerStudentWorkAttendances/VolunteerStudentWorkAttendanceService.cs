using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Data;
using VolunteerWorkApi.Dtos.VolunteerStudentWorkAttendance;
using VolunteerWorkApi.Extensions;
using VolunteerWorkApi.Helpers.ErrorHandling;

namespace VolunteerWorkApi.Services.VolunteerStudentWorkAttendances
{
    public class VolunteerStudentWorkAttendanceService
        : IVolunteerStudentWorkAttendanceService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public VolunteerStudentWorkAttendanceService(
            ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IEnumerable<VolunteerStudentWorkAttendanceDto> GetAll()
        {
            return _dbContext.VolunteerStudentWorkAttendances
                .Include(x => x.VolunteerStudent)
                .Include(x => x.VolunteerProgramWorkDay)
                .Select(x => _mapper.Map<VolunteerStudentWorkAttendanceDto>(x))
                .ToList();
        }

        public IEnumerable<VolunteerStudentWorkAttendanceDto> GetList(
            string? filter, int? skipCount, int? maxResultCount,
            long? volunteerStudentId, long? workDayId, bool? isAttended)
        {
            return _dbContext.VolunteerStudentWorkAttendances
                 .Include(x => x.VolunteerStudent)
                 .Include(x => x.VolunteerProgramWorkDay)
                 .WhereIf(volunteerStudentId != null,
                    x => x.VolunteerStudentId == volunteerStudentId)
                 .WhereIf(workDayId != null,
                    x => x.VolunteerProgramWorkDayId == workDayId)
                 .WhereIf(isAttended != null,
                    x => x.IsAttended == isAttended)
                 .Skip(skipCount ?? 0)
                 .Take(maxResultCount ?? ApiConstants.MaxResultCount)
                 .Select(x => _mapper.Map<VolunteerStudentWorkAttendanceDto>(x))
                 .ToList();
        }

        public VolunteerStudentWorkAttendanceDto GetById(long id)
        {
            var data = _dbContext.VolunteerStudentWorkAttendances.Find(id);

            if (data == null)
            {
                throw new ApiNotFoundException();
            }

            return _mapper.Map<VolunteerStudentWorkAttendanceDto>(data);
        }

        public async Task<VolunteerStudentWorkAttendanceDto> Create(
            CreateVolunteerStudentWorkAttendanceDto createEntityDto,
            long currentUserId)
        {
            var entity = _mapper.Map<VolunteerStudentWorkAttendance>(createEntityDto);

            entity.CreatedBy = currentUserId;

            var addedEntity = await _dbContext.VolunteerStudentWorkAttendances.AddAsync(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            var item = _dbContext.VolunteerStudentWorkAttendances
                      .Include(x => x.VolunteerStudent)
                      .Include(x => x.VolunteerProgramWorkDay)
                      .Where(x => x.Id == addedEntity.Entity.Id)
                      .FirstOrDefault();

            return _mapper.Map<VolunteerStudentWorkAttendanceDto>(item);
        }

        public async Task<VolunteerStudentWorkAttendanceDto> Update(
            UpdateVolunteerStudentWorkAttendanceDto updateEntityDto,
            long currentUserId)
        {
            var entity = _dbContext.VolunteerStudentWorkAttendances
                      .Include(x => x.VolunteerStudent)
                      .Include(x => x.VolunteerProgramWorkDay)
                      .Where(x => x.Id == updateEntityDto.Id)
                      .FirstOrDefault();

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity = _mapper.Map(updateEntityDto, entity);

            entity.ModifiedDate = DateTime.UtcNow;

            entity.ModifiedBy = currentUserId;

            _dbContext.VolunteerStudentWorkAttendances.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<VolunteerStudentWorkAttendanceDto>(entity);
        }

        public async Task<VolunteerStudentWorkAttendanceDto> Remove(long id)
        {
            var entity = _dbContext.VolunteerStudentWorkAttendances.Find(id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity.IsDeleted = true;

            _dbContext.VolunteerStudentWorkAttendances.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<VolunteerStudentWorkAttendanceDto>(entity);
        }
    }
}
