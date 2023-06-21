using AutoMapper;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Data;
using VolunteerWorkApi.Dtos.Category;
using VolunteerWorkApi.Extensions;
using VolunteerWorkApi.Helpers.ErrorHandling;

namespace VolunteerWorkApi.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public CategoryService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IEnumerable<CategoryDto> GetAll()
        {
            return _dbContext.Categories
                .Select(x => _mapper.Map<CategoryDto>(x))
                .ToList();
        }

        public IEnumerable<CategoryDto> GetList(string? filter,
            int? skipCount, int? maxResultCount)
        {
            return _dbContext.Categories
                 .WhereIf(!string.IsNullOrEmpty(filter),
                    x => x.Name.Contains(filter!))
                 .Skip(skipCount ?? 0)
                 .Take(maxResultCount ?? ApiConstants.MaxResultCount)
                 .Select(x => _mapper.Map<CategoryDto>(x))
                 .ToList();
        }

        public CategoryDto GetById(long id)
        {
            var data = _dbContext.Categories.Find(id);

            if (data == null)
            {
                throw new ApiNotFoundException();
            }

            return _mapper.Map<CategoryDto>(data);
        }

        public async Task<CategoryDto> Create(
            CreateCategoryDto createEntityDto, long currentUserId)
        {
            var entity = _mapper.Map<Category>(createEntityDto);

            entity.CreatedBy = currentUserId;

            await _dbContext.Categories.AddAsync(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<CategoryDto>(entity);
        }

        public async Task<CategoryDto> Update(
            UpdateCategoryDto updateEntityDto, long currentUserId)
        {
            var entity = _dbContext.Categories.Find(updateEntityDto.Id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity = _mapper.Map(updateEntityDto, entity);

            entity.ModifiedDate = DateTime.UtcNow;

            entity.ModifiedBy = currentUserId;

            _dbContext.Categories.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<CategoryDto>(entity);
        }

        public async Task<CategoryDto> Remove(long id)
        {
            var entity = _dbContext.Categories.Find(id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity.IsDeleted = true;

            _dbContext.Categories.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<CategoryDto>(entity);
        }
    }
}
