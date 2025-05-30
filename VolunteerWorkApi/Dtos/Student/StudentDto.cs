﻿using VolunteerWorkApi.Dtos.ApplicationUser;
using VolunteerWorkApi.Dtos.Skill;

namespace VolunteerWorkApi.Dtos.Student
{
    public record StudentDto : ApplicationUserDto
    {
        public int UniversityIdNumber { get; set; }

        public string? Specialization { get; set; }

        public string? Address { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Biography { get; set; }

        public bool IsEnrolledInProgram { get; set; }

        public ICollection<SkillDto> Skills { get; set; }
    }
}
