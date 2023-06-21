using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Data;
using VolunteerWorkApi.Dtos.Interest;
using VolunteerWorkApi.Dtos.Skill;
using VolunteerWorkApi.Extensions;
using VolunteerWorkApi.Helpers.ErrorHandling;

namespace VolunteerWorkApi.Services.Interests
{
    public class InterestService : IInterestService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public InterestService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IEnumerable<InterestDto> GetAll()
        {
            return _dbContext.Interests
                .Include(x => x.Category)
                .Select(x => _mapper.Map<InterestDto>(x))
                .ToList();
        }

        public IEnumerable<InterestDto> GetList(
            string? filter, int? skipCount, int? maxResultCount, long? studentId)
        {
            return _dbContext.Interests
                 .Include(x => x.Category)
                 .WhereIf(studentId != null,
                    x => x.Students.Any(s => s.Id == studentId))
                 .WhereIf(!string.IsNullOrEmpty(filter),
                    x => x.Name.Contains(filter!))
                 .Skip(skipCount ?? 0)
                 .Take(maxResultCount ?? ApiConstants.MaxResultCount)
                 .Select(x => _mapper.Map<InterestDto>(x))
                 .ToList();
        }

        public InterestDto GetById(long id)
        {
            var data = _dbContext.Skills.Find(id);

            if (data == null)
            {
                throw new ApiNotFoundException();
            }

            return _mapper.Map<InterestDto>(data);
        }

        public Interest GetEntityById(long id)
        {
            var entity = _dbContext.Interests.Find(id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            return entity;
        }

        public Interest? GetEntityByName(string name)
        {
            return _dbContext.Interests.FirstOrDefault(x => x.Name == name);
        }

        public async Task<InterestDto> Create(
            CreateInterestDto createEntityDto, long currentUserId)
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

            var entity = _mapper.Map<Interest>(createEntityDto);

            entity.CreatedBy = currentUserId;

            await _dbContext.Interests.AddAsync(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<InterestDto>(entity);
        }

        public async Task<Interest> CreateEntity(
           CreateInterestDto createEntityDto, long currentUserId)
        {
            var entity = _mapper.Map<Interest>(createEntityDto);

            entity.CreatedBy = currentUserId;

            await _dbContext.Interests.AddAsync(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return entity;
        }

        public async Task<InterestDto> Update(
            UpdateInterestDto updateEntityDto, long currentUserId)
        {
            var entity = _dbContext.Interests.Find(updateEntityDto.Id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity = _mapper.Map(updateEntityDto, entity);

            entity.ModifiedDate = DateTime.UtcNow;

            entity.ModifiedBy = currentUserId;

            _dbContext.Interests.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<InterestDto>(entity);
        }

        public async Task<InterestDto> Remove(long id)
        {
            var entity = _dbContext.Interests.Find(id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity.IsDeleted = true;

            _dbContext.Interests.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<InterestDto>(entity);
        }
    }
}
