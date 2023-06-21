
using VolunteerWorkApi.Dtos.Announcement;

namespace VolunteerWorkApi.Services.Announcements
{
    public interface IAnnouncementService
    {
        IEnumerable<AnnouncementDto> GetAll();

        IEnumerable<AnnouncementDto> GetList(
            string? filter, int? skipCount, int? maxResultCount,
            bool? isManagementAnnouncement,
            bool? isOrganizationAnnouncement,
            long? organizationId, long? volunteerProgramId);

        AnnouncementDto GetById(long id);

        Task<AnnouncementDto> Create(
            CreateAnnouncementDto createEntityDto, long currentUserId);

        Task<AnnouncementDto> Update(
            UpdateAnnouncementDto updateEntityDto, long currentUserId);

        Task<AnnouncementDto> Remove(long id);
    }
}
