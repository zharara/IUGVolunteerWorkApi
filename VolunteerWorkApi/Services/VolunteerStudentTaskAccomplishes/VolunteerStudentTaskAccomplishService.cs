using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Data;
using VolunteerWorkApi.Dtos.VolunteerStudentTaskAccomplish;
using VolunteerWorkApi.Extensions;
using VolunteerWorkApi.Helpers.ErrorHandling;

namespace VolunteerWorkApi.Services.VolunteerStudentTaskAccomplishes
{
    public class VolunteerStudentTaskAccomplishService 
        : IVolunteerStudentTaskAccomplishService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public VolunteerStudentTaskAccomplishService(
            ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IEnumerable<VolunteerStudentTaskAccomplishDto> GetAll()
        {
            return _dbContext.VolunteerStudentTaskAccomplishes
                .Include(x => x.VolunteerStudent)
                .Include(x => x.VolunteerProgramTask)
                .Select(x => _mapper.Map<VolunteerStudentTaskAccomplishDto>(x))
                .ToList();
        }

        public IEnumerable<VolunteerStudentTaskAccomplishDto> GetList(
            string? filter, int? skipCount, int? maxResultCount,
            long? volunteerStudentId, long? taskId, bool? isAccomplished)
        {
            return _dbContext.VolunteerStudentTaskAccomplishes
                 .Include(x => x.VolunteerStudent)
                 .Include(x => x.VolunteerProgramTask)
                 .WhereIf(volunteerStudentId != null,
                    x => x.VolunteerStudentId == volunteerStudentId)
                 .WhereIf(taskId != null,
                    x => x.VolunteerProgramTaskId == taskId)
                 .WhereIf(isAccomplished != null,
                    x => x.IsAccomplished == isAccomplished)
                 .Skip(skipCount ?? 0)
                 .Take(maxResultCount ?? ApiConstants.MaxResultCount)
                 .Select(x => _mapper.Map<VolunteerStudentTaskAccomplishDto>(x))
                 .ToList();
        }

        public VolunteerStudentTaskAccomplishDto GetById(long id)
        {
            var data = _dbContext.VolunteerStudentTaskAccomplishes.Find(id);

            if (data == null)
            {
                throw new ApiNotFoundException();
            }

            return _mapper.Map<VolunteerStudentTaskAccomplishDto>(data);
        }

        public async Task<VolunteerStudentTaskAccomplishDto> Create(
            CreateVolunteerStudentTaskAccomplishDto createEntityDto,
            long currentUserId)
        {
            var entity = _mapper.Map<VolunteerStudentTaskAccomplish>(createEntityDto);

            entity.CreatedBy = currentUserId;

            await _dbContext.VolunteerStudentTaskAccomplishes.AddAsync(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<VolunteerStudentTaskAccomplishDto>(entity);
        }

        public async Task<VolunteerStudentTaskAccomplishDto> Update(
            UpdateVolunteerStudentTaskAccomplishDto updateEntityDto,
            long currentUserId)
        {
            var entity = _dbContext.VolunteerStudentTaskAccomplishes.Find(updateEntityDto.Id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity = _mapper.Map(updateEntityDto, entity);

            entity.ModifiedDate = DateTime.UtcNow;

            entity.ModifiedBy = currentUserId;

            _dbContext.VolunteerStudentTaskAccomplishes.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<VolunteerStudentTaskAccomplishDto>(entity);
        }

        public async Task<VolunteerStudentTaskAccomplishDto> Remove(long id)
        {
            var entity = _dbContext.VolunteerStudentTaskAccomplishes.Find(id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity.IsDeleted = true;

            _dbContext.VolunteerStudentTaskAccomplishes.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<VolunteerStudentTaskAccomplishDto>(entity);
        }
    }
}
