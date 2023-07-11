using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Data;
using VolunteerWorkApi.Dtos.StudentApplication;
using VolunteerWorkApi.Enums;
using VolunteerWorkApi.Extensions;
using VolunteerWorkApi.Helpers.ErrorHandling;
using VolunteerWorkApi.Models;
using VolunteerWorkApi.Services.FCMNotifications;
using VolunteerWorkApi.Services.Notifications;
using VolunteerWorkApi.Services.SavedFiles;
using VolunteerWorkApi.Services.VolunteerOpportunities;

namespace VolunteerWorkApi.Services.StudentApplications
{
    public class StudentApplicationService : IStudentApplicationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ISavedFileService _savedFileService;
        private readonly IVolunteerOpportunityService _volunteerOpportunityService;
        private readonly INotificationService _notificationService;
        private readonly IFCMNotificationsService _fCMNotificationsService;

        public StudentApplicationService(
            ApplicationDbContext dbContext,
            IMapper mapper, ISavedFileService savedFileService,
            IVolunteerOpportunityService volunteerOpportunityService,
            INotificationService notificationService,
            IFCMNotificationsService fCMNotificationsService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _savedFileService = savedFileService;
            _volunteerOpportunityService = volunteerOpportunityService;
            _notificationService = notificationService;
            _fCMNotificationsService = fCMNotificationsService;
        }

        public IEnumerable<StudentApplicationDto> GetAll()
        {
            return _dbContext.StudentApplications
                .Include(x => x.Student)
                .Include(x => x.VolunteerOpportunity)
                .ThenInclude(x => x.Organization)
                .ThenInclude(x => x.ProfilePicture)
                .Include(x => x.VolunteerOpportunity)
                .ThenInclude(x => x.Category)
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
                 .ThenInclude(x => x.ProfilePicture)
                 .Include(x => x.VolunteerOpportunity)
                 .ThenInclude(x => x.Category)
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
                 .Include(x => x.VolunteerOpportunity)
                 .ThenInclude(x => x.Organization)
                 .ThenInclude(x => x.ProfilePicture)
                 .Include(x => x.VolunteerOpportunity)
                 .ThenInclude(x => x.Category)
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
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var entity = _dbContext.StudentApplications
                .Include(x => x.Student)
                 .Include(x => x.VolunteerOpportunity)
                 .ThenInclude(x => x.Organization)
                 .ThenInclude(x => x.ProfilePicture)
                 .Include(x => x.VolunteerOpportunity)
                .ThenInclude(x => x.Category)
                .Where(x => x.Id == studentApplicationId)
                .FirstOrDefault();

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

                await _notificationService.Create(new CreateNotification
                {
                    Title = NotificationsMessages.VolunteerApplicationAccepted,
                    Body = NotificationsMessages.ApplicationAcceptedByOrganization,
                    ItemId = entity.Id,
                    Page = NotificationPage.Notifications,
                    ApplicationUserId = entity.Student.Id,
                });

                if (!string.IsNullOrEmpty(entity.Student.FCMToken))
                {
                    await _fCMNotificationsService.SendNotification(new FCMNotification
                    {
                        FCMToken = entity.Student.FCMToken!,
                        Title = NotificationsMessages.VolunteerApplicationAccepted,
                        Body = NotificationsMessages.ApplicationAcceptedByOrganization,
                        ItemId = entity.Id,
                        Page = NotificationPage.Notifications,
                    });
                }

                List<ManagementEmployee> managementAccounts = _dbContext.ManagementEmployees.ToList();

                foreach (ManagementEmployee managementEmployee in managementAccounts)
                {
                    await _notificationService.Create(new CreateNotification
                    {
                        Title = NotificationsMessages.VolunteerApplicationAccepted,
                        Body = NotificationsMessages.ApplicationAcceptedByOrganization,
                        ItemId = entity.Id,
                        Page = NotificationPage.Notifications,
                        ApplicationUserId = managementEmployee.Id,
                    });

                    if (!string.IsNullOrEmpty(
                        managementEmployee.FCMToken))
                    {
                        await _fCMNotificationsService.SendNotification(new FCMNotification
                        {
                            FCMToken = managementEmployee.FCMToken!,
                            Title = NotificationsMessages.VolunteerApplicationAccepted,
                            Body = NotificationsMessages.ApplicationAcceptedByOrganization,
                            ItemId = entity.Id,
                            Page = NotificationPage.Notifications,
                        });
                    }
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

        public async Task<StudentApplicationDto> OrganizationRejectApplication(
            RejectStudentApplication rejectStudentApplicationDto,
            long organizationId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var entity = _dbContext.StudentApplications
                .Include(x => x.Student)
                 .Include(x => x.VolunteerOpportunity)
                 .ThenInclude(x => x.Organization)
                 .ThenInclude(x => x.ProfilePicture)
                 .Include(x => x.VolunteerOpportunity)
                .ThenInclude(x => x.Category)
                .Where(x => x.Id == rejectStudentApplicationDto.StudentApplicationId)
                .FirstOrDefault();

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

                await _notificationService.Create(new CreateNotification
                {
                    Title = NotificationsMessages.VolunteerApplicationRejected,
                    Body = NotificationsMessages.ApplicationRejectedByOrganization,
                    ItemId = entity.Id,
                    Page = NotificationPage.Notifications,
                    ApplicationUserId = entity.Student.Id,
                });

                if (!string.IsNullOrEmpty(entity.Student.FCMToken))
                {
                    await _fCMNotificationsService.SendNotification(new FCMNotification
                    {
                        FCMToken = entity.Student.FCMToken!,
                        Title = NotificationsMessages.VolunteerApplicationRejected,
                        Body = NotificationsMessages.ApplicationRejectedByOrganization,
                        ItemId = entity.Id,
                        Page = NotificationPage.Notifications,
                    });
                }

                List<ManagementEmployee> managementAccounts = _dbContext.ManagementEmployees.ToList();

                foreach (ManagementEmployee managementEmployee in managementAccounts)
                {
                    await _notificationService.Create(new CreateNotification
                    {
                        Title = NotificationsMessages.VolunteerApplicationRejected,
                        Body = NotificationsMessages.ApplicationRejectedByOrganization,
                        ItemId = entity.Id,
                        Page = NotificationPage.Notifications,
                        ApplicationUserId = managementEmployee.Id,
                    });

                    if (!string.IsNullOrEmpty(
                        managementEmployee.FCMToken))
                    {
                        await _fCMNotificationsService.SendNotification(new FCMNotification
                        {
                            FCMToken = managementEmployee.FCMToken!,
                            Title = NotificationsMessages.VolunteerApplicationRejected,
                            Body = NotificationsMessages.ApplicationRejectedByOrganization,
                            ItemId = entity.Id,
                            Page = NotificationPage.Notifications,
                        });
                    }
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

        public async Task<StudentApplicationDto> ManagementAcceptApplication(
            long studentApplicationId, long currentUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var entity = _dbContext.StudentApplications
                .Include(x => x.Student)
                 .Include(x => x.VolunteerOpportunity)
                 .ThenInclude(x => x.Organization)
                 .ThenInclude(x => x.ProfilePicture)
                 .Include(x => x.VolunteerOpportunity)
                .ThenInclude(x => x.Category)
                .Where(x => x.Id == studentApplicationId)
                .FirstOrDefault();

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

                await _notificationService.Create(new CreateNotification
                {
                    Title = NotificationsMessages.VolunteerApplicationAccepted,
                    Body = NotificationsMessages.ApplicationAcceptedByManagement,
                    ItemId = entity.Id,
                    Page = NotificationPage.Notifications,
                    ApplicationUserId = entity.Student.Id,
                });

                await _notificationService.Create(new CreateNotification
                {
                    Title = NotificationsMessages.VolunteerApplicationAccepted,
                    Body = NotificationsMessages.ApplicationAcceptedByManagement,
                    ItemId = entity.Id,
                    Page = NotificationPage.Notifications,
                    ApplicationUserId =
                    entity.VolunteerOpportunity.Organization.Id,
                });

                if (!string.IsNullOrEmpty(entity.Student.FCMToken))
                {
                    await _fCMNotificationsService.SendNotification(new FCMNotification
                    {
                        FCMToken = entity.Student.FCMToken!,
                        Title = NotificationsMessages.VolunteerApplicationAccepted,
                        Body = NotificationsMessages.ApplicationAcceptedByManagement,
                        ItemId = entity.Id,
                        Page = NotificationPage.Notifications,
                    });
                }

                if (!string.IsNullOrEmpty(
                    entity.VolunteerOpportunity.Organization.FCMToken))
                {
                    await _fCMNotificationsService.SendNotification(new FCMNotification
                    {
                        FCMToken = entity.VolunteerOpportunity.Organization.FCMToken!,
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
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var entity = _dbContext.StudentApplications
                    .Include(x => x.Student)
                     .Include(x => x.VolunteerOpportunity)
                     .ThenInclude(x => x.Organization)
                     .ThenInclude(x => x.ProfilePicture)
                     .Include(x => x.VolunteerOpportunity)
                    .ThenInclude(x => x.Category)
                    .Where(x => x.Id == rejectStudentApplicationDto.StudentApplicationId)
                    .FirstOrDefault();

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

                await _notificationService.Create(new CreateNotification
                {
                    Title = NotificationsMessages.VolunteerApplicationRejected,
                    Body = NotificationsMessages.ApplicationRejectedByManagement,
                    ItemId = entity.Id,
                    Page = NotificationPage.Notifications,
                    ApplicationUserId = entity.Student.Id,
                });

                await _notificationService.Create(new CreateNotification
                {
                    Title = NotificationsMessages.VolunteerApplicationRejected,
                    Body = NotificationsMessages.ApplicationRejectedByManagement,
                    ItemId = entity.Id,
                    Page = NotificationPage.Notifications,
                    ApplicationUserId =
                    entity.VolunteerOpportunity.Organization.Id,
                });

                if (!string.IsNullOrEmpty(entity.Student.FCMToken))
                {
                    await _fCMNotificationsService.SendNotification(new FCMNotification
                    {
                        FCMToken = entity.Student.FCMToken!,
                        Title = NotificationsMessages.VolunteerApplicationRejected,
                        Body = NotificationsMessages.ApplicationRejectedByManagement,
                        ItemId = entity.Id,
                        Page = NotificationPage.Notifications,
                    });
                }

                if (!string.IsNullOrEmpty(
                    entity.VolunteerOpportunity.Organization.FCMToken))
                {
                    await _fCMNotificationsService.SendNotification(new FCMNotification
                    {
                        FCMToken = entity.VolunteerOpportunity.Organization.FCMToken!,
                        Title = NotificationsMessages.VolunteerApplicationRejected,
                        Body = NotificationsMessages.ApplicationRejectedByManagement,
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


                var item = _dbContext.StudentApplications
                        .Include(x => x.Student)
                         .Include(x => x.VolunteerOpportunity)
                         .ThenInclude(x => x.Organization)
                         .ThenInclude(x => x.ProfilePicture)
                         .Include(x => x.VolunteerOpportunity)
                        .ThenInclude(x => x.Category)
                        .Where(x => x.Id == entity.Id)
                        .FirstOrDefault();

                await _notificationService.Create(new CreateNotification
                {
                    Title = NotificationsMessages.NewVolunteerApplication,
                    Body = NotificationsMessages.NewVolunteerApplicationToOpportunity,
                    ItemId = entity.Id,
                    Page = NotificationPage.Notifications,
                    ApplicationUserId = item.VolunteerOpportunity.Organization.Id,
                });

                if (!string.IsNullOrEmpty(item.VolunteerOpportunity.Organization.FCMToken))
                {
                    await _fCMNotificationsService.SendNotification(new FCMNotification
                    {
                        FCMToken = item.VolunteerOpportunity.Organization.FCMToken!,
                        Title = NotificationsMessages.NewVolunteerApplication,
                        Body = NotificationsMessages.NewVolunteerApplicationToOpportunity,
                        ItemId = entity.Id,
                        Page = NotificationPage.Notifications,
                    });
                }

                List<ManagementEmployee> managementAccounts = _dbContext.ManagementEmployees.ToList();

                foreach (ManagementEmployee managementEmployee in managementAccounts)
                {
                    await _notificationService.Create(new CreateNotification
                    {
                        Title = NotificationsMessages.NewVolunteerApplication,
                        Body = NotificationsMessages.NewVolunteerApplicationToOpportunity,
                        ItemId = entity.Id,
                        Page = NotificationPage.Notifications,
                        ApplicationUserId = managementEmployee.Id,
                    });

                    if (!string.IsNullOrEmpty(
                        managementEmployee.FCMToken))
                    {
                        await _fCMNotificationsService.SendNotification(new FCMNotification
                        {
                            FCMToken = managementEmployee.FCMToken!,
                            Title = NotificationsMessages.NewVolunteerApplication,
                            Body = NotificationsMessages.NewVolunteerApplicationToOpportunity,
                            ItemId = entity.Id,
                            Page = NotificationPage.Notifications,
                        });
                    }
                }

                transaction.Commit();

                return _mapper.Map<StudentApplicationDto>(item);
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
                var entity = _dbContext.StudentApplications
                    .Include(x => x.Student)
                     .Include(x => x.VolunteerOpportunity)
                     .ThenInclude(x => x.Organization)
                     .ThenInclude(x => x.ProfilePicture)
                     .Include(x => x.VolunteerOpportunity)
                    .ThenInclude(x => x.Category)
                    .Where(x => x.Id == updateEntityDto.Id)
                    .FirstOrDefault();

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
