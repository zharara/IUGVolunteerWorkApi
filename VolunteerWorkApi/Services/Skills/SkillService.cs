using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Data;
using VolunteerWorkApi.Dtos.Skill;
using VolunteerWorkApi.Extensions;
using VolunteerWorkApi.Helpers.ErrorHandling;

namespace VolunteerWorkApi.Services.Skills
{
    public class SkillService : ISkillService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public SkillService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IEnumerable<SkillDto> GetAll()
        {
            return _dbContext.Skills
                .Include(x => x.Category)
                .Select(x => _mapper.Map<SkillDto>(x))
                .ToList();
        }

        public IEnumerable<SkillDto> GetList(
            string? filter, int? skipCount, int? maxResultCount, long? studentId)
        {
            return _dbContext.Skills
                 .Include(x => x.Category)
                 .WhereIf(studentId != null,
                    x => x.Students.Any(s => s.Id == studentId))
                 .WhereIf(!string.IsNullOrEmpty(filter),
                    x => x.Name.Contains(filter!))
                 .Skip(skipCount ?? 0)
                 .Take(maxResultCount ?? ApiConstants.MaxResultCount)
                 .Select(x => _mapper.Map<SkillDto>(x))
                 .ToList();
        }

        public SkillDto GetById(long id)
        {
            var data = _dbContext.Skills.Find(id);

            if (data == null)
            {
                throw new ApiNotFoundException();
            }

            return _mapper.Map<SkillDto>(data);
        }

        public Skill GetEntityById(long id)
        {
            var entity = _dbContext.Skills.Find(id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            return entity;
        }

        public Skill? GetEntityByName(string name)
        {
            return _dbContext.Skills
                .Include(x => x.Category)
                .FirstOrDefault(x => x.Name == name);
        }

        public async Task<SkillDto> Create(
            CreateSkillDto createEntityDto, long currentUserId)
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

            var entity = _mapper.Map<Skill>(createEntityDto);

            entity.CreatedBy = currentUserId;

            var addedEntity = await _dbContext.Skills.AddAsync(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            var item = _dbContext.Skills
                      .Include(x => x.Category)
                      .Where(x => x.Id == addedEntity.Entity.Id)
                      .FirstOrDefault();

            return _mapper.Map<SkillDto>(item);
        }

        public async Task<Skill> CreateEntity(
           CreateSkillDto createEntityDto, long currentUserId)
        {
            var entity = _mapper.Map<Skill>(createEntityDto);

            entity.CreatedBy = currentUserId;

            var addedEntity = await _dbContext.Skills.AddAsync(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            var item = _dbContext.Skills
                      .Include(x => x.Category)
                      .Where(x => x.Id == addedEntity.Entity.Id)
                      .FirstOrDefault();

            return item!;
        }

        public async Task<SkillDto> Update(
            UpdateSkillDto updateEntityDto, long currentUserId)
        {
            var entity = _dbContext.Skills
                      .Include(x => x.Category)
                      .Where(x => x.Id == updateEntityDto.Id)
                      .FirstOrDefault();

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity = _mapper.Map(updateEntityDto, entity);

            entity.ModifiedDate = DateTime.UtcNow;

            entity.ModifiedBy = currentUserId;

            _dbContext.Skills.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<SkillDto>(entity);
        }

        public async Task<SkillDto> Remove(long id)
        {
            var entity = _dbContext.Skills.Find(id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity.IsDeleted = true;

            _dbContext.Skills.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<SkillDto>(entity);
        }
    }
}
