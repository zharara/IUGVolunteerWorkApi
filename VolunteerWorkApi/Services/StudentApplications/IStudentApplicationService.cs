using VolunteerWorkApi.Dtos.StudentApplication;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Services.StudentApplications
{
    public interface IStudentApplicationService
    {
        IEnumerable<StudentApplicationDto> GetAll();

        IEnumerable<StudentApplicationDto> GetList(
            int? skipCount, int? maxResultCount,
            long? studentId, long? volunteerOpportunityId,
            long? organizationId);

        IEnumerable<StudentApplication> GetListOfOpportunity(
           long volunteerOpportunityId);

        StudentApplicationDto GetById(long id);

        Task<StudentApplicationDto> OrganizationAcceptApplication(
            long studentApplicationId, long organizationId);

        Task<StudentApplicationDto> OrganizationRejectApplication(
            RejectStudentApplication rejectStudentApplicationDto,
            long organizationId);

        Task<StudentApplicationDto> ManagementAcceptApplication(
            long studentApplicationId, long currentUserId);

        Task<StudentApplicationDto> ManagementRejectApplication(
         RejectStudentApplication rejectStudentApplicationDto,
         long currentUserId);

        Task<StudentApplicationDto> Create(
            CreateStudentApplicationDto createEntityDto, long currentUserId);

        Task<StudentApplicationDto> Update(
            UpdateStudentApplicationDto updateEntityDto, long currentUserId);

        Task<StudentApplicationDto> Remove(long id);
    }
}
