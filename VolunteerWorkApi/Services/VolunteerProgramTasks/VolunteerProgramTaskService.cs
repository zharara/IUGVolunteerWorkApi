using AutoMapper;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Data;
using VolunteerWorkApi.Dtos.VolunteerProgramTask;
using VolunteerWorkApi.Extensions;
using VolunteerWorkApi.Helpers.ErrorHandling;

namespace VolunteerWorkApi.Services.VolunteerProgramTasks
{
    public class VolunteerProgramTaskService : IVolunteerProgramTaskService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public VolunteerProgramTaskService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IEnumerable<VolunteerProgramTaskDto> GetAll()
        {
            return _dbContext.VolunteerProgramTasks
                .Select(x => _mapper.Map<VolunteerProgramTaskDto>(x))
                .ToList();
        }

        public IEnumerable<VolunteerProgramTaskDto> GetList(string? filter,
            int? skipCount, int? maxResultCount, long? volunteerProgramId)
        {
            return _dbContext.VolunteerProgramTasks
                 .WhereIf(volunteerProgramId != null,
                    x => x.VolunteerProgramId == volunteerProgramId)
                 .WhereIf(!string.IsNullOrEmpty(filter),
                    x => x.Title.Contains(filter!))
                 .Skip(skipCount ?? 0)
                 .Take(maxResultCount ?? ApiConstants.MaxResultCount)
                 .Select(x => _mapper.Map<VolunteerProgramTaskDto>(x))
                 .ToList();
        }

        public VolunteerProgramTaskDto GetById(long id)
        {
            var data = _dbContext.VolunteerProgramTasks.Find(id);

            if (data == null)
            {
                throw new ApiNotFoundException();
            }

            return _mapper.Map<VolunteerProgramTaskDto>(data);
        }

        public async Task<VolunteerProgramTaskDto> Create(
            CreateVolunteerProgramTaskDto createEntityDto, long currentUserId)
        {
            var entity = _mapper.Map<VolunteerProgramTask>(createEntityDto);

            entity.CreatedBy = currentUserId;

            await _dbContext.VolunteerProgramTasks.AddAsync(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<VolunteerProgramTaskDto>(entity);
        }

        public async Task<VolunteerProgramTaskDto> Update(
            UpdateVolunteerProgramTaskDto updateEntityDto, long currentUserId)
        {
            var entity = _dbContext.VolunteerProgramTasks.Find(updateEntityDto.Id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity = _mapper.Map(updateEntityDto, entity);

            entity.ModifiedDate = DateTime.UtcNow;

            entity.ModifiedBy = currentUserId;

            _dbContext.VolunteerProgramTasks.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<VolunteerProgramTaskDto>(entity);
        }

        public async Task<VolunteerProgramTaskDto> Remove(long id)
        {
            var entity = _dbContext.VolunteerProgramTasks.Find(id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity.IsDeleted = true;

            _dbContext.VolunteerProgramTasks.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<VolunteerProgramTaskDto>(entity);
        }
    }
}
