using AutoMapper;
using System.Net;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Dtos.Interest;
using VolunteerWorkApi.Dtos.Skill;
using VolunteerWorkApi.Entities;
using VolunteerWorkApi.Helpers.ErrorHandling;
using VolunteerWorkApi.Services.Interests;
using VolunteerWorkApi.Services.Skills;

namespace VolunteerWorkApi.Helpers
{
    public static class DataCollectionsHandler
    {
        public static void HandleInterests(
              IMapper mapper,
              IInterestService interestService,
              ICollection<Interest> entityInterests,
              ICollection<ExistingOrCreateNewInterestDto> toSetInterests)
        {

            var selectedInterestsIds = new List<long>();

            foreach (ExistingOrCreateNewInterestDto interestToSet
                in toSetInterests)
            {
                if (interestToSet.Id != null)
                {
                    selectedInterestsIds.Add((long)interestToSet.Id);
                }

                var existingStudentInterest = entityInterests
                    .FirstOrDefault(x => x.Id == (interestToSet.Id ?? -1));

                if (existingStudentInterest == null)
                {
                    Interest interest;

                    if (interestToSet.Id != null)
                    {
                        interest = interestService
                            .GetEntityById((long)interestToSet.Id);
                    }
                    else
                    {
                        bool isCategoryNotSetProperly =
                            (interestToSet.CategoryId == null
                            && interestToSet.Category is null) ||
                            (interestToSet.CategoryId != null
                            && interestToSet.Category is not null);

                        if (string.IsNullOrEmpty(interestToSet.Name) ||
                             isCategoryNotSetProperly)
                        {
                            throw new ApiResponseException(
                                HttpStatusCode.BadRequest,
                                ErrorMessages.InputError,
                                ErrorMessages.InputConflict);
                        }

                        interest = new Interest
                        {
                            Name = interestToSet.Name,
                            CategoryId = interestToSet.CategoryId ?? 0,
                            Category = mapper.Map<Category>(interestToSet.Category),
                        };
                    }

                    entityInterests.Add(interest);
                }
            }

            foreach (var interest in entityInterests)
            {
                if (interest.Id != 0 && !selectedInterestsIds.Contains(interest.Id))
                {
                    entityInterests.Remove(interest);
                }
            }
        }

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
