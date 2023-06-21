using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Dtos.Student
{
    public record UpdateStudentDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long Id { get; set; }

        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }

        public string? Specialization { get; set; }

        public string? Address { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Biography { get; set; }

        public SaveTempFile? SaveTempFile { get; set; }
    }
}
