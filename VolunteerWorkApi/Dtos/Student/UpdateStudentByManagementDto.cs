using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;

namespace VolunteerWorkApi.Dtos.Student
{
    public record UpdateStudentByManagementDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long Id { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public int UniversityIdNumber { get; set; }

        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }

        public string? Specialization { get; set; }

        public string? Address { get; set; }

        public DateTime? DateOfBirth { get; set; }
    }
}
