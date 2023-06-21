using VolunteerWorkApi.Dtos.VolunteerProgramGalleryPhoto;

namespace VolunteerWorkApi.Services.VolunteerProgramGalleryPhotos
{
    public interface IVolunteerProgramGalleryPhotoService
    {
        IEnumerable<VolunteerProgramGalleryPhotoDto> GetAll();

        IEnumerable<VolunteerProgramGalleryPhotoDto> GetList(
            int? skipCount, int? maxResultCount,
            long? volunteerProgramId, bool? isApproved);

        VolunteerProgramGalleryPhotoDto GetById(long id);

        Task<VolunteerProgramGalleryPhotoDto> Create(
            CreateGalleryPhotoDto createEntityDto,
            long currentUserId);

        Task<VolunteerProgramGalleryPhotoDto> CreateGalleryPhotoByStudent(
            CreateGalleryPhotoByStudentDto createEntityDto,
            long currentUserId);

        Task<VolunteerProgramGalleryPhotoDto> Update(
            UpdateGalleryPhotoDto updateEntityDto,
            long currentUserId);

        Task<VolunteerProgramGalleryPhotoDto> UpdateGalleryPhotoByStudent(
            UpdateGalleryPhotoByStudentDto updateEntityDto,
            long currentUserId);

        Task<VolunteerProgramGalleryPhotoDto> Remove(long id);
    }
}
