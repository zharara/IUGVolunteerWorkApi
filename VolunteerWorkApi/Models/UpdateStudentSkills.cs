using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Dtos.Skill;

namespace VolunteerWorkApi.Models
{
    public record UpdateStudentSkills
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long StudentId { get; set; }

        public ICollection<ExistingOrCreateNewSkillDto> Skills { get; set; }
    }
}
