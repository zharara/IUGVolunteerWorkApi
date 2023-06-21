using VolunteerWorkApi.Dtos.Skill;

namespace VolunteerWorkApi.Services.Skills
{
    public interface ISkillService
    {
        IEnumerable<SkillDto> GetAll();

        IEnumerable<SkillDto> GetList(
            string? filter, int? skipCount,
            int? maxResultCount, long? studentId);

        SkillDto GetById(long id);

        Skill GetEntityById(long id);

        Skill? GetEntityByName(string name);

        Task<SkillDto> Create(CreateSkillDto createEntityDto, long currentUserId);

        Task<Skill> CreateEntity(CreateSkillDto createEntityDto, long currentUserId);

        Task<SkillDto> Update(UpdateSkillDto updateEntityDto, long currentUserId);

        Task<SkillDto> Remove(long id);
    }
}
