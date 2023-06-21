using AutoMapper;
using VolunteerWorkApi.Data;
using VolunteerWorkApi.Dtos.ManagementEmployee;
using VolunteerWorkApi.Extensions;
using VolunteerWorkApi.Services.Users;
using VolunteerWorkApi.Models;
using VolunteerWorkApi.Constants;
using Microsoft.EntityFrameworkCore;
using VolunteerWorkApi.Helpers.ErrorHandling;
using VolunteerWorkApi.Services.SavedFiles;

namespace VolunteerWorkApi.Services.ManagementEmployees
{
    public class ManagementEmployeeService : IManagementEmployeeService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUsersService _usersService;
        private readonly ISavedFileService _savedFileService;

        public ManagementEmployeeService(
            ApplicationDbContext dbContext, IMapper mapper,
            IUsersService usersService, ISavedFileService savedFileService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _usersService = usersService;
            _savedFileService = savedFileService;
        }

        public IEnumerable<ManagementEmployeeDto> GetAll()
        {
            return _dbContext.ManagementEmployees
                .Include(x => x.ProfilePicture)
                .Select(x => _mapper.Map<ManagementEmployeeDto>(x))
                .ToList();
        }

        public IEnumerable<ManagementEmployeeDto> GetList(
            string? filter, int? skipCount, int? maxResultCount)
        {
            return _dbContext.ManagementEmployees
                 .Include(x => x.ProfilePicture)
                 .WhereIf(!string.IsNullOrEmpty(filter),
                    x => x.FullName.Contains(filter!))
                 .Skip(skipCount ?? 0)
                 .Take(maxResultCount ?? ApiConstants.MaxResultCount)
                 .Select(x => _mapper.Map<ManagementEmployeeDto>(x))
                 .ToList();
        }

        public ManagementEmployeeDto GetById(long id)
        {
            var data = _dbContext.ManagementEmployees.Find(id);

            if (data == null)
            {
                throw new ApiNotFoundException();
            }

            return _mapper.Map<ManagementEmployeeDto>(data);
        }

        public async Task<ManagementEmployeeAccount> Create(
            CreateManagementEmployeeDto createEntityDto)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                ManagementEmployee entity =
                _mapper.Map<ManagementEmployee>(createEntityDto);

                if (createEntityDto.SaveTempFile != null)
                {
                    var savedFile = await _savedFileService
                         .Create(createEntityDto.SaveTempFile);

                    entity.ProfilePicture = savedFile;
                }

                var cratedUser = await _usersService.CreateManagement(
                    new CreateAccount(
                            entity,
                            createEntityDto.Password
                        ));

                var dto = _mapper.Map<ManagementEmployeeDto>(
                    cratedUser.ApplicationUser);

                transaction.Commit();

                return new ManagementEmployeeAccount(
                    dto, cratedUser.AuthToken);
            }
            catch
            {
                transaction.Rollback();

                throw;
            }
        }

        public async Task<ManagementEmployeeDto> Update(
            UpdateManagementEmployeeDto updateEntityDto, long currentUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var entity = _dbContext.ManagementEmployees
                    .Find(updateEntityDto.Id);

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

                _dbContext.ManagementEmployees.Update(entity);

                int effectedRows = await _dbContext.SaveChangesAsync();

                if (!(effectedRows > 0))
                {
                    throw new ApiDataException();
                }

                transaction.Commit();

                return _mapper.Map<ManagementEmployeeDto>(entity);
            }
            catch
            {
                transaction.Rollback();

                throw;
            }
        }

        public async Task<ManagementEmployeeDto> Remove(long id)
        {
            var entity = _dbContext.ManagementEmployees.Find(id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity.IsDeleted = true;

            _dbContext.ManagementEmployees.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<ManagementEmployeeDto>(entity);
        }
    }
}
