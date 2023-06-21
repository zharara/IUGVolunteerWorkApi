﻿using VolunteerWorkApi.Dtos.Category;
using VolunteerWorkApi.Dtos.Interest;
using VolunteerWorkApi.Dtos.Organization;
using VolunteerWorkApi.Dtos.SavedFile;
using VolunteerWorkApi.Dtos.Skill;

namespace VolunteerWorkApi.Dtos.VolunteerOpportunity
{
    public record VolunteerOpportunityDto
    {
        public long Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public long OrganizationId { get; set; }

        public OrganizationDto Organization { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string NatureOfWorkOrActivities { get; set; }

        public long CategoryId { get; set; }

        public CategoryDto Category { get; set; }

        public SavedFileDto? Logo { get; set; }

        public DateTime ActualProgramStartDate { get; set; }

        public DateTime ActualProgramEndDate { get; set; }

        public DateTime? AnnouncementEndDate { get; set; }

        public DateTime ReceiveApplicationsEndDate { get; set; }

        public int RequiredVolunteerStudentsNumber { get; set; }

        public ICollection<InterestDto>? VolunteerInterests { get; set; }

        public ICollection<SkillDto>? VolunteerSkills { get; set; }

        public string? ApplicantQualifications { get; set; }

        public bool IsRequirementNeededAsText { get; set; }

        public bool IsRequirementNeededAsFile { get; set; }

        public string? RequirementFileDescription { get; set; }

        public double? RequirementFileMaxAllowedSize { get; set; }

        public string? RequirementFileAllowedTypes { get; set; }
    }
}
