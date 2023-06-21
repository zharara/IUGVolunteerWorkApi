using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Data;
using VolunteerWorkApi.Dtos.StudentApplication;
using VolunteerWorkApi.Dtos.VolunteerStudent;
using VolunteerWorkApi.Enums;
using VolunteerWorkApi.Extensions;
using VolunteerWorkApi.Helpers.ErrorHandling;
using VolunteerWorkApi.Models;
using VolunteerWorkApi.Services.FCMNotifications;
using VolunteerWorkApi.Services.Notifications;
using VolunteerWorkApi.Services.SavedFiles;
using VolunteerWorkApi.Services.Students;
using VolunteerWorkApi.Services.VolunteerOpportunities;

namespace VolunteerWorkApi.Services.StudentApplications
{
    public class StudentApplicationService : IStudentApplicationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ISavedFileService _savedFileService;
        private readonly IVolunteerOpportunityService _volunteerOpportunityService;
        private readonly IStudentService _studentService;
        private readonly INotificationService _notificationService;
        private readonly IFCMNotificationsService _fCMNotificationsService;

        public StudentApplicationService(
            ApplicationDbContext dbContext,
            IMapper mapper, ISavedFileService savedFileService,
            IVolunteerOpportunityService volunteerOpportunityService,
            IStudentService studentService,
            INotificationService notificationService,
            IFCMNotificationsService fCMNotificationsService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _savedFileService = savedFileService;
            _volunteerOpportunityService = volunteerOpportunityService;
            _studentService = studentService;
            _notificationService = notificationService;
            _fCMNotificationsService = fCMNotificationsService;
        }

        public IEnumerable<StudentApplicationDto> GetAll()
        {
            return _dbContext.StudentApplications
                .Include(x => x.Student)
                .Include(x => x.VolunteerOpportunity)
                .ThenInclude(x => x.Organization)
                .Select(x => _mapper.Map<StudentApplicationDto>(x))
                .ToList();
        }

        public IEnumerable<StudentApplicationDto> GetList(
            int? skipCount, int? maxResultCount,
            long? studentId, long? volunteerOpportunityId,
            long? organizationId)
        {
            return _dbContext.StudentApplications
                 .Include(x => x.Student)
                 .Include(x => x.VolunteerOpportunity)
                 .ThenInclude(x => x.Organization)
                 .WhereIf(studentId != null,
                    x => x.StudentId == studentId)
                 .WhereIf(volunteerOpportunityId != null,
                    x => x.VolunteerOpportunityId == volunteerOpportunityId)
                 .WhereIf(organizationId != null,
                   x => x.VolunteerOpportunity.OrganizationId == organizationId)
                 .Skip(skipCount ?? 0)
                 .Take(maxResultCount ?? ApiConstants.MaxResultCount)
                 .Select(x => _mapper.Map<StudentApplicationDto>(x))
                 .ToList();
        }

        public IEnumerable<StudentApplication> GetListOfOpportunity(
            long volunteerOpportunityId)
        {
            return _dbContext.StudentApplications
                .Include(x => x.Student)
                .Where(x => x.VolunteerOpportunityId == volunteerOpportunityId)
                .ToList();
        }

        public StudentApplicationDto GetById(long id)
        {
            var data = _dbContext.StudentApplications.Find(id);

            if (data == null)
            {
                throw new ApiNotFoundException();
            }

            return _mapper.Map<StudentApplicationDto>(data);
        }

        public async Task<StudentApplicationDto> OrganizationAcceptApplication(
            long studentApplicationId, long organizationId)
        {
            var entity = _dbContext.StudentApplications.Find(studentApplicationId);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            if (entity.VolunteerOpportunity.OrganizationId != organizationId)
            {
                throw new ApiResponseException(
                    HttpStatusCode.Unauthorized,
                    ErrorMessages.AuthError,
                    ErrorMessages.NoPermissionsForAccount);
            }

            entity.StatusForOrganization = ApplicationStatus.Approved;

            entity.ModifiedDate = DateTime.UtcNow;

            entity.ModifiedBy = organizationId;

            _dbContext.StudentApplications.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<StudentApplicationDto>(entity);
        }

        public async Task<StudentApplicationDto> OrganizationRejectApplication(
            RejectStudentApplication rejectStudentApplicationDto,
            long organizationId)
        {
            var entity = _dbContext.StudentApplications
                .Find(rejectStudentApplicationDto.StudentApplicationId);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            if (entity.VolunteerOpportunity.OrganizationId != organizationId)
            {
                throw new ApiResponseException(
                     HttpStatusCode.Unauthorized,
                    ErrorMessages.AuthError,
                    ErrorMessages.NoPermissionsForAccount);
            }

            if (string.IsNullOrEmpty(rejectStudentApplicationDto.RejectionReason))
            {
                throw new ApiResponseException(
                     HttpStatusCode.BadRequest,
                    ErrorMessages.InputError,
                    ErrorMessages.MustStateRejectionReason);
            }

            entity.StatusForOrganization = ApplicationStatus.Rejected;

            entity.OrganizationRejectionReason =
                rejectStudentApplicationDto.RejectionReason;

            entity.ModifiedDate = DateTime.UtcNow;

            entity.ModifiedBy = organizationId;

            _dbContext.StudentApplications.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<StudentApplicationDto>(entity);
        }

        public async Task<StudentApplicationDto> ManagementAcceptApplication(
            long studentApplicationId, long currentUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var entity = _dbContext.StudentApplications.Find(studentApplicationId);

                if (entity == null)
                {
                    throw new ApiNotFoundException();
                }

                if (entity.StatusForManagement == ApplicationStatus.Approved)
                {
                    throw new ApiResponseException(
                         HttpStatusCode.BadRequest,
                         ErrorMessages.DataError,
                         ErrorMessages.ApplicationIsAlreadyApproved);
                }

                entity.StatusForManagement = ApplicationStatus.Approved;

                entity.ModifiedDate = DateTime.UtcNow;

                entity.ModifiedBy = currentUserId;

                _dbContext.StudentApplications.Update(entity);

                int effectedRows = await _dbContext.SaveChangesAsync();

                if (!(effectedRows > 0))
                {
                    throw new ApiDataException();
                }

                var student = _studentService.GetById(entity.StudentId);

                await _notificationService.Create(new CreateNotification
                {
                    Title = NotificationsMessages.VolunteerApplicationAccepted,
                    Body = NotificationsMessages.ApplicationAcceptedByManagement,
                    ItemId = entity.Id,
                    Page = NotificationPage.Notifications,
                    ApplicationUserId = student.Id,
                });

                if (!string.IsNullOrEmpty(student.FCMToken))
                {
                    _fCMNotificationsService.SendNotification(new FCMNotification
                    {
                        FCMToken = student.FCMToken!,
                        Title = NotificationsMessages.VolunteerApplicationAccepted,
                        Body = NotificationsMessages.ApplicationAcceptedByManagement,
                        ItemId = entity.Id,
                        Page = NotificationPage.Notifications,
                    });
                }

                transaction.Commit();

                return _mapper.Map<StudentApplicationDto>(entity);
            }
            catch
            {
                transaction.Rollback();

                throw;
            }
        }

        public async Task<StudentApplicationDto> ManagementRejectApplication(
         RejectStudentApplication rejectStudentApplicationDto,
         long currentUserId)
        {
            var entity = _dbContext.StudentApplications
                .Find(rejectStudentApplicationDto.StudentApplicationId);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity.StatusForManagement = ApplicationStatus.Rejected;

            entity.ManagementRejectionReason =
                rejectStudentApplicationDto.RejectionReason;

            entity.ModifiedDate = DateTime.UtcNow;

            entity.ModifiedBy = currentUserId;

            _dbContext.StudentApplications.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<StudentApplicationDto>(entity);
        }

        public async Task<StudentApplicationDto> Create(
            CreateStudentApplicationDto createEntityDto, long currentUserId)
        {
            var opportunityDto = _volunteerOpportunityService
                 .GetById(createEntityDto.VolunteerOpportunityId);

            if ((opportunityDto.IsRequirementNeededAsText &&
                string.IsNullOrEmpty(createEntityDto.TextInformation)) ||
                (opportunityDto.IsRequirementNeededAsFile &&
                createEntityDto.SaveTempFile == null))
            {
                throw new ApiResponseException(
                     HttpStatusCode.BadRequest,
                    ErrorMessages.InputError,
                    ErrorMessages.MakeSureToFulfillApplicationRequirements);
            }

            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var entity = _mapper.Map<StudentApplication>(createEntityDto);

                if (createEntityDto.SaveTempFile != null)
                {
                    var savedFile = await _savedFileService
                         .Create(createEntityDto.SaveTempFile);

                    entity.SubmittedFile = savedFile;
                }

                entity.StatusForManagement = ApplicationStatus.Pending;
                entity.StatusForOrganization = ApplicationStatus.Pending;

                entity.CreatedBy = currentUserId;

                await _dbContext.StudentApplications.AddAsync(entity);

                int effectedRows = await _dbContext.SaveChangesAsync();

                if (!(effectedRows > 0))
                {
                    throw new ApiDataException();
                }

                transaction.Commit();

                return _mapper.Map<StudentApplicationDto>(entity);
            }
            catch
            {
                transaction.Rollback();

                throw;
            }
        }

        public async Task<StudentApplicationDto> Update(
            UpdateStudentApplicationDto updateEntityDto, long currentUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var entity = _dbContext.StudentApplications.Find(updateEntityDto.Id);

                if (entity == null)
                {
                    throw new ApiNotFoundException();
                }

                entity = _mapper.Map(updateEntityDto, entity);

                if (updateEntityDto.SaveTempFile != null)
                {
                    var savedFile = await _savedFileService
                         .Create(updateEntityDto.SaveTempFile);

                    entity.SubmittedFile = savedFile;
                }

                entity.ModifiedDate = DateTime.UtcNow;

                entity.ModifiedBy = currentUserId;

                _dbContext.StudentApplications.Update(entity);

                int effectedRows = await _dbContext.SaveChangesAsync();

                if (!(effectedRows > 0))
                {
                    throw new ApiDataException();
                }

                transaction.Commit();

                return _mapper.Map<StudentApplicationDto>(entity);
            }
            catch
            {
                transaction.Rollback();

                throw;
            }
        }

        public async Task<StudentApplicationDto> Remove(long id)
        {
            var entity = _dbContext.StudentApplications.Find(id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity.IsDeleted = true;

            _dbContext.StudentApplications.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<StudentApplicationDto>(entity);
        }
    }
}
