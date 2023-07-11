using AutoMapper;
using System.Net;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Dtos.Skill;
using VolunteerWorkApi.Helpers.ErrorHandling;
using VolunteerWorkApi.Services.Skills;

namespace VolunteerWorkApi.Helpers
{
    public static class DataCollectionsHandler
    {
        public static void HandleSkills(
          IMapper mapper,
          ISkillService skillService,
          ICollection<Skill>? entitySkills,
          ICollection<ExistingOrCreateNewSkillDto> toSetSkills)
        {
            var selectedSkillsIds = new List<long>();

            foreach (ExistingOrCreateNewSkillDto skillToSet
                in toSetSkills)
            {
                if (skillToSet.Id != null)
                {
                    selectedSkillsIds.Add((long)skillToSet.Id);
                }

                var existingStudentSkill = entitySkills
                    .FirstOrDefault(x => x.Id == (skillToSet.Id ?? -1));

                if (existingStudentSkill == null)
                {
                    Skill skill;

                    if (skillToSet.Id != null)
                    {
                        skill = skillService
                            .GetEntityById((long)skillToSet.Id);
                    }
                    else
                    {
                        bool isCategoryNotSetProperly =
                            (skillToSet.CategoryId == null
                            && skillToSet.Category is null) ||
                            (skillToSet.CategoryId != null
                            && skillToSet.Category is not null);

                        if (string.IsNullOrEmpty(skillToSet.Name) ||
                             isCategoryNotSetProperly)
                        {
                            throw new ApiResponseException(
                                HttpStatusCode.BadRequest,
                                ErrorMessages.InputError,
                                ErrorMessages.InputConflict);
                        }

                        skill = new Skill
                        {
                            Name = skillToSet.Name,
                            CategoryId = skillToSet.CategoryId ?? 0,
                            Category = mapper.Map<Category>(skillToSet.Category),
                        };
                    }

                    entitySkills.Add(skill);
                }
            }

            foreach (var skill in entitySkills)
            {
                if (skill.Id != 0 && !selectedSkillsIds.Contains(skill.Id))
                {
                    entitySkills.Remove(skill);
                }
            }
        }
    }
}
