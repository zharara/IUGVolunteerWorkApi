using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Data;
using VolunteerWorkApi.Dtos.VolunteerStudentActivityAttendance;
using VolunteerWorkApi.Extensions;
using VolunteerWorkApi.Helpers.ErrorHandling;

namespace VolunteerWorkApi.Services.VolunteerStudentActivityAttendances
{
    public class VolunteerStudentActivityAttendanceService
        : IVolunteerStudentActivityAttendanceService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public VolunteerStudentActivityAttendanceService(
            ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IEnumerable<VolunteerStudentActivityAttendanceDto> GetAll()
        {
            return _dbContext.VolunteerStudentActivityAttendances
                .Include(x => x.VolunteerStudent)
                .Include(x => x.VolunteerProgramActivity)
                .Select(x => _mapper.Map<VolunteerStudentActivityAttendanceDto>(x))
                .ToList();
        }

        public IEnumerable<VolunteerStudentActivityAttendanceDto> GetList(
            string? filter, int? skipCount, int? maxResultCount,
            long? volunteerStudentId, long? activityId, bool? isAttended)
        {
            return _dbContext.VolunteerStudentActivityAttendances
                 .Include(x => x.VolunteerStudent)
                 .Include(x => x.VolunteerProgramActivity)
                 .WhereIf(volunteerStudentId != null,
                    x => x.VolunteerStudentId == volunteerStudentId)
                 .WhereIf(activityId != null,
                    x => x.VolunteerProgramActivityId == activityId)
                 .WhereIf(isAttended != null,
                    x => x.IsAttended == isAttended)
                 .Skip(skipCount ?? 0)
                 .Take(maxResultCount ?? ApiConstants.MaxResultCount)
                 .Select(x => _mapper.Map<VolunteerStudentActivityAttendanceDto>(x))
                 .ToList();
        }

        public VolunteerStudentActivityAttendanceDto GetById(long id)
        {
            var data = _dbContext.VolunteerStudentActivityAttendances.Find(id);

            if (data == null)
            {
                throw new ApiNotFoundException();
            }

            return _mapper.Map<VolunteerStudentActivityAttendanceDto>(data);
        }

        public async Task<VolunteerStudentActivityAttendanceDto> Create(
            CreateVolunteerStudentActivityAttendanceDto createEntityDto,
            long currentUserId)
        {
            var entity = _mapper.Map<VolunteerStudentActivityAttendance>(createEntityDto);

            entity.CreatedBy = currentUserId;

            await _dbContext.VolunteerStudentActivityAttendances.AddAsync(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<VolunteerStudentActivityAttendanceDto>(entity);
        }

        public async Task<VolunteerStudentActivityAttendanceDto> Update(
            UpdateVolunteerStudentActivityAttendanceDto updateEntityDto,
            long currentUserId)
        {
            var entity = _dbContext.VolunteerStudentActivityAttendances.Find(updateEntityDto.Id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity = _mapper.Map(updateEntityDto, entity);

            entity.ModifiedDate = DateTime.UtcNow;

            entity.ModifiedBy = currentUserId;

            _dbContext.VolunteerStudentActivityAttendances.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<VolunteerStudentActivityAttendanceDto>(entity);
        }

        public async Task<VolunteerStudentActivityAttendanceDto> Remove(long id)
        {
            var entity = _dbContext.VolunteerStudentActivityAttendances.Find(id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity.IsDeleted = true;

            _dbContext.VolunteerStudentActivityAttendances.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<VolunteerStudentActivityAttendanceDto>(entity);
        }
    }
}
