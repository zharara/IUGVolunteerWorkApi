using System.ComponentModel.DataAnnotations;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Dtos.Category;
using VolunteerWorkApi.Dtos.Interest;
using VolunteerWorkApi.Dtos.Skill;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Dtos.VolunteerOpportunity
{
    public record CreateVolunteerOpportunityDto
    {
        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public long OrganizationId { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public string Title { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public string Description { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public string NatureOfWorkOrActivities { get; set; }

        public long? CategoryId { get; set; }

        public CreateCategoryDto? Category { get; set; }

        public SaveTempFile? SaveTempFile { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public DateTime ActualProgramStartDate { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public DateTime ActualProgramEndDate { get; set; }

        public DateTime? AnnouncementEndDate { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public DateTime ReceiveApplicationsEndDate { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public int RequiredVolunteerStudentsNumber { get; set; }

        public ICollection<ExistingOrCreateNewInterestDto>? VolunteerInterests { get; set; }

        public ICollection<ExistingOrCreateNewSkillDto>? VolunteerSkills { get; set; }

        public string? ApplicantQualifications { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public bool IsRequirementNeededAsText { get; set; }

        [Required(ErrorMessage = ErrorMessages.FieldRequired)]
        public bool IsRequirementNeededAsFile { get; set; }

        public string? RequirementFileDescription { get; set; }

        public double? RequirementFileMaxAllowedSize { get; set; }

        public string? RequirementFileAllowedTypes { get; set; }
    }
}
