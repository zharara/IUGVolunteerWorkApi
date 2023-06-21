using AutoMapper;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Data;
using VolunteerWorkApi.Dtos.VolunteerProgramWorkDay;
using VolunteerWorkApi.Extensions;
using VolunteerWorkApi.Helpers.ErrorHandling;

namespace VolunteerWorkApi.Services.VolunteerProgramWorkDays
{
    public class VolunteerProgramWorkDayService : IVolunteerProgramWorkDayService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public VolunteerProgramWorkDayService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IEnumerable<VolunteerProgramWorkDayDto> GetAll()
        {
            return _dbContext.VolunteerProgramWorkDays
                .Select(x => _mapper.Map<VolunteerProgramWorkDayDto>(x))
                .ToList();
        }

        public IEnumerable<VolunteerProgramWorkDayDto> GetList(
            int? skipCount, int? maxResultCount, long? volunteerProgramId,
            DateTime? startDate, DateTime? endDate)
        {
            return _dbContext.VolunteerProgramWorkDays
                 .WhereIf(volunteerProgramId != null,
                    x => x.VolunteerProgramId == volunteerProgramId)
                 .WhereIf(startDate is DateTime,
                    x => x.CreatedDate.Date >= ((DateTime)startDate!).Date)
                 .WhereIf(endDate is DateTime,
                    x => x.CreatedDate.Date <= ((DateTime)endDate!).Date)
                 .Skip(skipCount ?? 0)
                 .Take(maxResultCount ?? ApiConstants.MaxResultCount)
                 .Select(x => _mapper.Map<VolunteerProgramWorkDayDto>(x))
                 .ToList();
        }

        public VolunteerProgramWorkDayDto GetById(long id)
        {
            var data = _dbContext.VolunteerProgramWorkDays.Find(id);

            if (data == null)
            {
                throw new ApiNotFoundException();
            }

            return _mapper.Map<VolunteerProgramWorkDayDto>(data);
        }

        public async Task<VolunteerProgramWorkDayDto> Create(
            CreateVolunteerProgramWorkDayDto createEntityDto, long currentUserId)
        {
            var entity = _mapper.Map<VolunteerProgramWorkDay>(createEntityDto);

            entity.CreatedBy = currentUserId;

            await _dbContext.VolunteerProgramWorkDays.AddAsync(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<VolunteerProgramWorkDayDto>(entity);
        }

        public async Task<VolunteerProgramWorkDayDto> Update(
            UpdateVolunteerProgramWorkDayDto updateEntityDto, long currentUserId)
        {
            var entity = _dbContext.VolunteerProgramWorkDays.Find(updateEntityDto.Id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity = _mapper.Map(updateEntityDto, entity);

            entity.ModifiedDate = DateTime.UtcNow;

            entity.ModifiedBy = currentUserId;

            _dbContext.VolunteerProgramWorkDays.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<VolunteerProgramWorkDayDto>(entity);
        }

        public async Task<VolunteerProgramWorkDayDto> Remove(long id)
        {
            var entity = _dbContext.VolunteerProgramWorkDays.Find(id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity.IsDeleted = true;

            _dbContext.VolunteerProgramWorkDays.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<VolunteerProgramWorkDayDto>(entity);
        }
    }
}
