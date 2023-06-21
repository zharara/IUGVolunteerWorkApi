using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Dtos.Category;
using VolunteerWorkApi.Dtos.Interest;
using VolunteerWorkApi.Dtos.Skill;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Dtos.VolunteerOpportunity
{
    public record UpdateVolunteerOpportunityDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? NatureOfWorkOrActivities { get; set; }

        public long? CategoryId { get; set; }

        public CreateCategoryDto? Category { get; set; }

        public SaveTempFile? SaveTempFile { get; set; }

        public DateTime? ActualProgramStartDate { get; set; }

        public DateTime? ActualProgramEndDate { get; set; }

        public DateTime? AnnouncementEndDate { get; set; }

        public DateTime? ReceiveApplicationsEndDate { get; set; }

        public int? RequiredVolunteerStudentsNumber { get; set; }

        public ICollection<ExistingOrCreateNewInterestDto>? VolunteerInterests { get; set; }

        public ICollection<ExistingOrCreateNewSkillDto>? VolunteerSkills { get; set; }

        public string? ApplicantQualifications { get; set; }

        public bool? IsRequirementNeededAsText { get; set; }

        public bool? IsRequirementNeededAsFile { get; set; }

        public string? RequirementFileDescription { get; set; }

        public double? RequirementFileMaxAllowedSize { get; set; }

        public string? RequirementFileAllowedTypes { get; set; }
    }
}
