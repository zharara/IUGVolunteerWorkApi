using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Data;
using VolunteerWorkApi.Dtos.Organization;
using VolunteerWorkApi.Extensions;
using VolunteerWorkApi.Helpers.ErrorHandling;
using VolunteerWorkApi.Models;
using VolunteerWorkApi.Services.SavedFiles;
using VolunteerWorkApi.Services.Users;

namespace VolunteerWorkApi.Services.Organizations
{
    public class OrganizationService : IOrganizationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUsersService _usersService;
        private readonly ISavedFileService _savedFileService;

        public OrganizationService(
            ApplicationDbContext dbContext,
            IMapper mapper,
            IUsersService usersService,
            ISavedFileService savedFileService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _usersService = usersService;
            _savedFileService = savedFileService;
        }

        public IEnumerable<OrganizationDto> GetAll()
        {
            return _dbContext.Organizations
                .Include(x => x.ProfilePicture)
                .Select(x => _mapper.Map<OrganizationDto>(x))
                .ToList();
        }

        public IEnumerable<OrganizationDto> GetList(
            string? filter, int? skipCount, int? maxResultCount)
        {
            return _dbContext.Organizations
                .Include(x => x.ProfilePicture)
                .WhereIf(!string.IsNullOrEmpty(filter),
                   x => x.FullName.Contains(filter!))
                .Skip(skipCount ?? 0)
                .Take(maxResultCount ?? ApiConstants.MaxResultCount)
                .Select(x => _mapper.Map<OrganizationDto>(x))
                .ToList();
        }

        public OrganizationDto GetById(long id)
        {
            var data = _dbContext.Organizations.Find(id);

            if (data == null)
            {
                throw new ApiNotFoundException();
            }

            return _mapper.Map<OrganizationDto>(data);
        }

        public async Task<OrganizationAccount> Create(
            CreateOrganizationDto createEntityDto)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                Organization entity =
                _mapper.Map<Organization>(createEntityDto);

                if (createEntityDto.SaveTempFile != null)
                {
                    var savedFile = await _savedFileService
                         .Create(createEntityDto.SaveTempFile);

                    entity.ProfilePicture = savedFile;
                }

                var cratedUser = await _usersService.CreateOrganization(
                    new CreateAccount(
                            entity,
                            createEntityDto.Password
                        ));

                var dto = _mapper.Map<OrganizationDto>(
                    cratedUser.ApplicationUser);

                transaction.Commit();

                return new OrganizationAccount(
                    dto, cratedUser.AuthToken);
            }
            catch
            {
                transaction.Rollback();

                throw;
            }
        }

        public async Task<OrganizationDto> Update(
            UpdateOrganizationDto updateEntityDto, long currentUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var entity = _dbContext.Organizations.Find(updateEntityDto.Id);

                if (entity == null)
                {
                    throw new ApiNotFoundException();
                }

                entity = _mapper.Map(updateEntityDto, entity);

                if (updateEntityDto.SaveTempFile != null)
                {
                    var savedFile = await _savedFileService
                         .Create(updateEntityDto.SaveTempFile);

                    entity.ProfilePicture = savedFile;
                }

                entity.ModifiedDate = DateTime.UtcNow;

                entity.ModifiedBy = currentUserId;

                _dbContext.Organizations.Update(entity);

                int effectedRows = await _dbContext.SaveChangesAsync();

                if (!(effectedRows > 0))
                {
                    throw new ApiDataException();
                }

                transaction.Commit();

                return _mapper.Map<OrganizationDto>(entity);
            }
            catch
            {
                transaction.Rollback();

                throw;
            }
        }

        public async Task<OrganizationDto> Remove(long id)
        {
            var entity = _dbContext.Organizations.Find(id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity.IsDeleted = true;

            _dbContext.Organizations.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<OrganizationDto>(entity);
        }
    }
}
