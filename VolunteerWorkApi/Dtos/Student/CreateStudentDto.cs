using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Dtos.Student
{
    public record CreateStudentDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public int UniversityIdNumber { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public string UserName { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public string Password { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public string FirstName { get; set; }

        public string? MiddleName { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public string LastName { get; set; }

        public string? Specialization { get; set; }

        public string? Address { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Biography { get; set; }

        public SaveTempFile? SaveTempFile { get; set; }

        public string? FCMToken { get; set; }
    }
}
