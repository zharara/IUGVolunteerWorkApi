using VolunteerWorkApi.Dtos.Category;

namespace VolunteerWorkApi.Services.Categories
{
    public interface ICategoryService
    {
        IEnumerable<CategoryDto> GetAll();

        IEnumerable<CategoryDto> GetList(
            string? filter, int? skipCount,
            int? maxResultCount);

        CategoryDto GetById(long id);

        Task<CategoryDto> Create(
            CreateCategoryDto createEntityDto, long currentUserId);

        Task<CategoryDto> Update(
            UpdateCategoryDto updateEntityDto, long currentUserId);

        Task<CategoryDto> Remove(long id);
    }
}
