using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VolunteerWorkApi.Data;
using VolunteerWorkApi.Dtos.VolunteerOpportunity;
using VolunteerWorkApi.Extensions;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Helpers.ErrorHandling;
using VolunteerWorkApi.Services.SavedFiles;
using VolunteerWorkApi.Helpers;
using VolunteerWorkApi.Services.Interests;
using VolunteerWorkApi.Services.Skills;
using System.Net;

namespace VolunteerWorkApi.Services.VolunteerOpportunities
{
    public class VolunteerOpportunityService : IVolunteerOpportunityService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ISavedFileService _savedFileService;
        private readonly ISkillService _skillService;
        private readonly IInterestService _interestService;

        public VolunteerOpportunityService(
            ApplicationDbContext dbContext,
            IMapper mapper, ISavedFileService savedFileService,
            ISkillService skillService, IInterestService interestService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _savedFileService = savedFileService;
            _skillService = skillService;
            _interestService = interestService;
        }

        public IEnumerable<VolunteerOpportunityDto> GetAll()
        {
            return _dbContext.VolunteerOpportunities
                .Include(x => x.Organization)
                .Include(x => x.Category)
                .Include(x => x.Logo)
                .Include(x => x.VolunteerInterests)
                .ThenInclude(x => x.Category)
                .Include(x => x.VolunteerSkills)
                .ThenInclude(x => x.Category)
                .Select(x => _mapper.Map<VolunteerOpportunityDto>(x))
                .ToList();
        }

        public IEnumerable<VolunteerOpportunityDto> GetList(string? filter,
            int? skipCount, int? maxResultCount, long? organizationId)
        {
            return _dbContext.VolunteerOpportunities
                 .Include(x => x.Organization)
                 .Include(x => x.Category)
                 .Include(x => x.Logo)
                 .Include(x => x.VolunteerInterests)
                 .ThenInclude(x => x.Category)
                 .Include(x => x.VolunteerSkills)
                 .ThenInclude(x => x.Category)
                 .WhereIf(organizationId != null,
                    x => x.OrganizationId == organizationId)
                 .WhereIf(!string.IsNullOrEmpty(filter),
                    x => x.Title.Contains(filter!))
                 .Skip(skipCount ?? 0)
                 .Take(maxResultCount ?? ApiConstants.MaxResultCount)
                 .Select(x => _mapper.Map<VolunteerOpportunityDto>(x))
                 .ToList();
        }

        public VolunteerOpportunityDto GetById(long id)
        {
            var entity = _dbContext.VolunteerOpportunities
                 .Include(x => x.Organization)
                 .Include(x => x.Category)
                 .Include(x => x.Logo)
                 .Include(x => x.VolunteerInterests)
                 .ThenInclude(x => x.Category)
                 .Include(x => x.VolunteerSkills)
                 .ThenInclude(x => x.Category)
                 .FirstOrDefault(x => x.Id == id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            return _mapper.Map<VolunteerOpportunityDto>(entity);
        }

        public async Task<VolunteerOpportunityDto> Create(
            CreateVolunteerOpportunityDto createEntityDto, long currentUserId)
        {
            if (createEntityDto.CategoryId == null
                && createEntityDto.Category is null)
            {
                throw new ApiResponseException(
                    HttpStatusCode.BadRequest,
                    ErrorMessages.InputError,
                    ErrorMessages.RequiredFieldsNotInputted,
                    nameof(createEntityDto.Category));
            }

            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var entity = _mapper.Map<VolunteerOpportunity>(createEntityDto);

                if (createEntityDto.SaveTempFile != null)
                {
                    var savedFile = await _savedFileService
                         .Create(createEntityDto.SaveTempFile);

                    entity.Logo = savedFile;
                }

                DataCollectionsHandler.HandleSkills(
                    _mapper, _skillService,
                    entity.VolunteerSkills,
                    createEntityDto.VolunteerSkills);

                DataCollectionsHandler.HandleInterests(
                    _mapper, _interestService,
                    entity.VolunteerInterests,
                    createEntityDto.VolunteerInterests);

                entity.CreatedBy = currentUserId;

                await _dbContext.VolunteerOpportunities.AddAsync(entity);

                _dbContext.Entry(entity).Reference(x => x.Organization).Load();
                _dbContext.Entry(entity).Reference(x => x.Category).Load();
                _dbContext.Entry(entity).Reference(x => x.Logo).Load();
                _dbContext.Entry(entity)
                    .Collection(x => x.VolunteerInterests)
                    .Query().Include(x => x.Category).Load();
                _dbContext.Entry(entity)
                    .Collection(x => x.VolunteerSkills)
                    .Query().Include(x => x.Category).Load();

                int effectedRows = await _dbContext.SaveChangesAsync();

                if (!(effectedRows > 0))
                {
                    throw new ApiDataException();
                }

                transaction.Commit();

                return _mapper.Map<VolunteerOpportunityDto>(entity);
            }
            catch
            {
                transaction.Rollback();

                throw;
            }
        }

        public async Task<VolunteerOpportunityDto> Update(
            UpdateVolunteerOpportunityDto updateEntityDto, long currentUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var entity = _dbContext.VolunteerOpportunities
                     .Include(x => x.Organization)
                     .Include(x => x.Category)
                     .Include(x => x.Logo)
                     .Include(x => x.VolunteerInterests)
                     .ThenInclude(x => x.Category)
                     .Include(x => x.VolunteerSkills)
                     .ThenInclude(x => x.Category)
                     .FirstOrDefault(x => x.Id == updateEntityDto.Id);

                if (entity == null)
                {
                    throw new ApiNotFoundException();
                }

                entity = _mapper.Map(updateEntityDto, entity);

                if (updateEntityDto.SaveTempFile != null)
                {
                    var savedFile = await _savedFileService
                         .Create(updateEntityDto.SaveTempFile);

                    entity.Logo = savedFile;
                }

                DataCollectionsHandler.HandleSkills(
                    _mapper, _skillService,
                    entity.VolunteerSkills,
                    updateEntityDto.VolunteerSkills);

                DataCollectionsHandler.HandleInterests(
                    _mapper, _interestService,
                    entity.VolunteerInterests,
                    updateEntityDto.VolunteerInterests);

                entity.ModifiedDate = DateTime.UtcNow;

                entity.ModifiedBy = currentUserId;

                _dbContext.VolunteerOpportunities.Update(entity);

                int effectedRows = await _dbContext.SaveChangesAsync();

                if (!(effectedRows > 0))
                {
                    throw new ApiDataException();
                }

                transaction.Commit();

                return _mapper.Map<VolunteerOpportunityDto>(entity);
            }
            catch
            {
                transaction.Rollback();

                throw;
            }
        }

        public async Task<VolunteerOpportunityDto> Remove(long id)
        {
            var entity = _dbContext.VolunteerOpportunities
                     .Include(x => x.Organization)
                     .Include(x => x.Category)
                     .Include(x => x.Logo)
                     .Include(x => x.VolunteerInterests)
                     .ThenInclude(x => x.Category)
                     .Include(x => x.VolunteerSkills)
                     .ThenInclude(x => x.Category)
                     .FirstOrDefault(x => x.Id == id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity.IsDeleted = true;

            _dbContext.VolunteerOpportunities.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<VolunteerOpportunityDto>(entity);
        }
    }
}
