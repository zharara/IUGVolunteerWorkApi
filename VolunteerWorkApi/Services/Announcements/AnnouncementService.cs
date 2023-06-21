using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Data;
using VolunteerWorkApi.Dtos.Announcement;
using VolunteerWorkApi.Extensions;
using VolunteerWorkApi.Helpers.ErrorHandling;
using VolunteerWorkApi.Services.SavedFiles;

namespace VolunteerWorkApi.Services.Announcements
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ISavedFileService _savedFileService;

        public AnnouncementService(
            ApplicationDbContext dbContext,
            IMapper mapper, ISavedFileService savedFileService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _savedFileService = savedFileService;
        }

        public IEnumerable<AnnouncementDto> GetAll()
        {
            return _dbContext.Announcements
                .Include(x => x.Image)
                .Select(x => _mapper.Map<AnnouncementDto>(x))
                .ToList();
        }

        public IEnumerable<AnnouncementDto> GetList(string? filter,
            int? skipCount, int? maxResultCount,
            bool? isManagementAnnouncement,
            bool? isOrganizationAnnouncement,
            long? organizationId, long? volunteerProgramId)
        {
            return _dbContext.Announcements
                 .Include(x => x.Image)
                 .WhereIf(!string.IsNullOrEmpty(filter),
                    x => x.Title.Contains(filter!))
                 .WhereIf(isManagementAnnouncement is true,
                    x => x.IsManagementAnnouncement)
                 .WhereIf(isOrganizationAnnouncement is true,
                    x => x.IsOrganizationAnnouncement)
                 .WhereIf(organizationId != null,
                    x => x.OrganizationId == organizationId)
                 .WhereIf(volunteerProgramId != null,
                    x => x.VolunteerProgramId == volunteerProgramId)
                 .Skip(skipCount ?? 0)
                 .Take(maxResultCount ?? ApiConstants.MaxResultCount)
                 .Select(x => _mapper.Map<AnnouncementDto>(x))
                 .ToList();
        }

        public AnnouncementDto GetById(long id)
        {
            var data = _dbContext.Announcements
                .Include(x => x.Image)
                .FirstOrDefault(x => x.Id == id);

            if (data == null)
            {
                throw new ApiNotFoundException();
            }

            return _mapper.Map<AnnouncementDto>(data);
        }

        public async Task<AnnouncementDto> Create(
            CreateAnnouncementDto createEntityDto, long currentUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var entity = _mapper.Map<Announcement>(createEntityDto);

                if (createEntityDto.SaveTempFile != null)
                {
                    var savedFile = await _savedFileService
                         .Create(createEntityDto.SaveTempFile);

                    entity.Image = savedFile;
                }

                entity.CreatedBy = currentUserId;

                await _dbContext.Announcements.AddAsync(entity);

                _dbContext.Entry(entity).Reference(x => x.Image).Load();

                int effectedRows = await _dbContext.SaveChangesAsync();

                if (!(effectedRows > 0))
                {
                    throw new ApiDataException();
                }

                transaction.Commit();

                return _mapper.Map<AnnouncementDto>(entity);
            }
            catch
            {
                transaction.Rollback();

                throw;
            }
        }

        public async Task<AnnouncementDto> Update(
            UpdateAnnouncementDto updateEntityDto, long currentUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var entity = _dbContext.Announcements
                    .Include(x => x.Image)
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

                    entity.Image = savedFile;
                }

                entity.ModifiedDate = DateTime.UtcNow;

                entity.ModifiedBy = currentUserId;

                _dbContext.Announcements.Update(entity);

                int effectedRows = await _dbContext.SaveChangesAsync();

                if (!(effectedRows > 0))
                {
                    throw new ApiDataException();
                }

                transaction.Commit();

                return _mapper.Map<AnnouncementDto>(entity);
            }
            catch
            {
                transaction.Rollback();

                throw;
            }
        }

        public async Task<AnnouncementDto> Remove(long id)
        {
            var entity = _dbContext.Announcements
                .Include(x => x.Image)
                .FirstOrDefault(x => x.Id == id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity.IsDeleted = true;

            _dbContext.Announcements.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<AnnouncementDto>(entity);
        }
    }
}
