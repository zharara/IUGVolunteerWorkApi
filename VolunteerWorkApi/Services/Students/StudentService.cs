using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Data;
using VolunteerWorkApi.Dtos.Student;
using VolunteerWorkApi.Extensions;
using VolunteerWorkApi.Helpers;
using VolunteerWorkApi.Helpers.ErrorHandling;
using VolunteerWorkApi.Models;
using VolunteerWorkApi.Services.SavedFiles;
using VolunteerWorkApi.Services.Skills;
using VolunteerWorkApi.Services.Users;

namespace VolunteerWorkApi.Services.Students
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUsersService _usersService;
        private readonly ISkillService _skillService;
        private readonly ISavedFileService _savedFileService;

        public StudentService(
            ApplicationDbContext dbContext,
            IMapper mapper,
            IUsersService usersService,
            ISkillService skillService,
            ISavedFileService savedFileService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _usersService = usersService;
            _skillService = skillService;
            _savedFileService = savedFileService;
        }

        public IEnumerable<StudentDto> GetAll()
        {
            return _dbContext.Students
                .Include(x => x.ProfilePicture)
                .Include(x => x.Skills)
                .Select(x => _mapper.Map<StudentDto>(x))
                .ToList();
        }

        public IEnumerable<StudentDto> GetList(
            string? filter, int? skipCount,
            int? maxResultCount, bool? isNotVolunteer)
        {
            return _dbContext.Students
                .Include(x => x.ProfilePicture)
                .Include(x => x.Skills)
                .WhereIf(!string.IsNullOrEmpty(filter),
                   x => x.FullName.Contains(filter!))
                .WhereIf(isNotVolunteer == true,
                   x => !x.IsEnrolledInProgram)
                .Skip(skipCount ?? 0)
                .Take(maxResultCount ?? ApiConstants.MaxResultCount)
                .Select(x => _mapper.Map<StudentDto>(x))
                .ToList();
        }

        public StudentDto GetById(long id)
        {
            var data = _dbContext.Students.Find(id);

            if (data == null)
            {
                throw new ApiNotFoundException();
            }

            return _mapper.Map<StudentDto>(data);
        }

        public async Task<StudentAccount> Create(
            CreateStudentDto createEntityDto,
            long currentUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                Student entity =
                _mapper.Map<Student>(createEntityDto);

                if (createEntityDto.SaveTempFile != null)
                {
                    var savedFile = await _savedFileService
                         .Create(createEntityDto.SaveTempFile);

                    entity.ProfilePicture = savedFile;
                }

                var cratedUser = await _usersService.CreateStudent(
                    new CreateAccount(
                            entity,
                            createEntityDto.Password
                        ));

                var dto = _mapper.Map<StudentDto>(
                    cratedUser.ApplicationUser);

                transaction.Commit();

                return new StudentAccount(
                    dto, cratedUser.AuthToken);
            }
            catch
            {
                transaction.Rollback();

                throw;
            }
        }

        public async Task<StudentDto> UpdateSkills(
            UpdateStudentSkills updateStudentSkillsDto, long currentUserId)
        {
            var entity = _dbContext.Students
                .Include(x => x.Skills)
                .FirstOrDefault(x => x.Id == updateStudentSkillsDto.StudentId);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            DataCollectionsHandler.HandleSkills(
                _mapper, _skillService,
                entity.Skills, updateStudentSkillsDto.Skills);

            entity.ModifiedDate = DateTime.UtcNow;

            entity.ModifiedBy = currentUserId;

            _dbContext.Students.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<StudentDto>(entity);
        }

        public async Task<StudentDto> Update(
            UpdateStudentDto updateEntityDto, long currentUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var entity = _dbContext.Students
                        .Include(x => x.ProfilePicture)
                        .Include(x => x.Skills)
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

                    entity.ProfilePicture = savedFile;
                }

                entity.ModifiedDate = DateTime.UtcNow;

                entity.ModifiedBy = currentUserId;

                _dbContext.Students.Update(entity);

                int effectedRows = await _dbContext.SaveChangesAsync();

                if (!(effectedRows > 0))
                {
                    throw new ApiDataException();
                }

                transaction.Commit();

                return _mapper.Map<StudentDto>(entity);
            }
            catch
            {
                transaction.Rollback();

                throw;
            }
        }

        public async Task<StudentDto> UpdateByManagement(
           UpdateStudentByManagementDto updateStudentByManagement,
           long currentUserId)
        {
            var entity = _dbContext.Students
                    .Include(x => x.ProfilePicture)
                    .Include(x => x.Skills)
                    .Where(x => x.Id == updateStudentByManagement.Id)
                    .FirstOrDefault();

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity = _mapper.Map(updateStudentByManagement, entity);

            entity.ModifiedDate = DateTime.UtcNow;

            entity.ModifiedBy = currentUserId;

            _dbContext.Students.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<StudentDto>(entity);
        }

        public async Task StudentHasEnrolledInProgram(
            long studentId)
        {
            var entity = _dbContext.Students.Find(studentId);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity.IsEnrolledInProgram = true;

            _dbContext.Students.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }
        }

        public async Task<StudentDto> Remove(long id)
        {
            var entity = _dbContext.Students.Find(id);

            if (entity == null)
            {
                throw new ApiNotFoundException();
            }

            entity.IsDeleted = true;

            _dbContext.Students.Update(entity);

            int effectedRows = await _dbContext.SaveChangesAsync();

            if (!(effectedRows > 0))
            {
                throw new ApiDataException();
            }

            return _mapper.Map<StudentDto>(entity);
        }
    }
}
