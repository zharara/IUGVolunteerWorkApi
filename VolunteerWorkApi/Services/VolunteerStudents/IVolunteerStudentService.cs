using VolunteerWorkApi.Dtos.VolunteerStudent;

namespace VolunteerWorkApi.Services.VolunteerStudents
{
    public interface IVolunteerStudentService
    {
        IEnumerable<VolunteerStudentDto> GetAll();

        IEnumerable<VolunteerStudentDto> GetList(
            string? filter, int? skipCount,
            int? maxResultCount, long? volunteerProgramId,
            long? organizationId);

        VolunteerStudentDto GetById(long id);

        Task<VolunteerStudentDto> Create(
            CreateVolunteerStudentDto createEntityDto, long currentUserId);

        Task<VolunteerStudent> CreateEntity(
            long studentId, long volunteerProgramId);

        Task<VolunteerStudentDto> UpdateOrganizationAssessment(
            UpdateVolunteerStudentOrgAssessmentDto updateEntityDto, long currentUserId);

        Task<VolunteerStudentDto> UpdateManagementFinalEvaluation(
          UpdateVolunteerStudentFinalEvaluationDto updateEntityDto, long currentUserId);

        Task<VolunteerStudentDto> Remove(long id);
    }
}
