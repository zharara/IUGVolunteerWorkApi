using VolunteerWorkApi.Dtos.Interest;
using VolunteerWorkApi.Dtos.Skill;

namespace VolunteerWorkApi.Services.Interests
{
    public interface IInterestService
    {
        IEnumerable<InterestDto> GetAll();

        IEnumerable<InterestDto> GetList(
            string? filter, int? skipCount,
            int? maxResultCount, long? studentId);

        InterestDto GetById(long id);

        Interest GetEntityById(long id);

        Interest? GetEntityByName(string name);

        Task<InterestDto> Create(
            CreateInterestDto createEntityDto, long currentUserId);

        Task<Interest> CreateEntity(
            CreateInterestDto createEntityDto, long currentUserId);

        Task<InterestDto> Update(
            UpdateInterestDto updateEntityDto, long currentUserId);

        Task<InterestDto> Remove(long id);
    }
}
