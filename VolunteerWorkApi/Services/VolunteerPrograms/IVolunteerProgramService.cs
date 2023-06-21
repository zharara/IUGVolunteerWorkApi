
using VolunteerWorkApi.Dtos.VolunteerProgram;

namespace VolunteerWorkApi.Services.VolunteerPrograms
{
    public interface IVolunteerProgramService
    {
        IEnumerable<VolunteerProgramDto> GetAll();

        IEnumerable<VolunteerProgramDto> GetList(
            string? filter, int? skipCount,
            int? maxResultCount, long? organizationId,
            bool? isInternalProgram);

        VolunteerProgramDto GetById(long id);

        Task<VolunteerProgramDto> Create(
            CreateVolunteerProgramDto createEntityDto, long currentUserId);


        Task<VolunteerProgramDto> CreateInternalProgram(
            CreateInternalVolunteerProgramDto createEntityDto, long currentUserId);

        Task<VolunteerProgramDto> Update(
            UpdateVolunteerProgramDto updateEntityDto, long currentUserId);

        Task<VolunteerProgramDto> UpdateInternalProgram(
            UpdateInternalVolunteerProgramDto updateEntityDto, long currentUserId);

        Task<VolunteerProgramDto> Remove(long id);
    }
}
