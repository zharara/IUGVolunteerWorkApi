using VolunteerWorkApi.Dtos.Student;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Services.Students
{
    public interface IStudentService
    {
        IEnumerable<StudentDto> GetAll();

        IEnumerable<StudentDto> GetList(
            string? filter, int? skipCount, int? maxResultCount);

        StudentDto GetById(long id);

        Task<StudentAccount> Create(
            CreateStudentDto createEntityDto,
            long currentUserId
            );

        Task<StudentDto> Update(
            UpdateStudentDto updateEntityDto, long currentUserId);

        Task<StudentDto> UpdateSkills(
           UpdateStudentSkills updateStudentSkillsDto, long currentUserId);

        Task<StudentDto> UpdateInterests(
           UpdateStudentInterests updateStudentInterestsDto, long currentUserId);

        Task<StudentDto> Remove(long id);
    }
}
