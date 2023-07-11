using AutoMapper;
using VolunteerWorkApi.Dtos.Announcement;
using VolunteerWorkApi.Dtos.Category;
using VolunteerWorkApi.Dtos.Conversation;
using VolunteerWorkApi.Dtos.Message;
using VolunteerWorkApi.Dtos.Notification;
using VolunteerWorkApi.Dtos.SavedFile;
using VolunteerWorkApi.Dtos.Skill;
using VolunteerWorkApi.Dtos.StudentApplication;
using VolunteerWorkApi.Dtos.TempFile;
using VolunteerWorkApi.Dtos.VolunteerOpportunity;
using VolunteerWorkApi.Dtos.VolunteerProgram;
using VolunteerWorkApi.Dtos.VolunteerProgramActivity;
using VolunteerWorkApi.Dtos.VolunteerProgramGalleryPhoto;
using VolunteerWorkApi.Dtos.VolunteerProgramTask;
using VolunteerWorkApi.Dtos.VolunteerProgramWorkDay;
using VolunteerWorkApi.Dtos.VolunteerStudent;
using VolunteerWorkApi.Dtos.VolunteerStudentActivityAttendance;
using VolunteerWorkApi.Dtos.VolunteerStudentTaskAccomplish;
using VolunteerWorkApi.Dtos.VolunteerStudentWorkAttendance;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.AutoMapper
{
    public class VolunteerWorkMapperProfile : Profile
    {
        public VolunteerWorkMapperProfile()
        {
            CreateMap<Announcement, CreateAnnouncementDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<Announcement, UpdateAnnouncementDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<Announcement, AnnouncementDto>().ReverseMap()
             .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));

            CreateMap<Category, CreateCategoryDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<Category, UpdateCategoryDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<Category, CategoryDto>().ReverseMap()
             .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));

            CreateMap<Conversation, CreateConversation>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<Conversation, ConversationDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));

            CreateMap<SavedFile, SavedFileDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
           
            CreateMap<TempFile, TempFileDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));

            CreateMap<Message, CreateMessageDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<Message, UpdateMessageDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<Message, MessageDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));

            CreateMap<Notification, CreateNotification>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<Notification, NotificationDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));

            CreateMap<Skill, CreateSkillDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<Skill, UpdateSkillDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<Skill, SkillDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<Skill, ExistingOrCreateNewSkillDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));

            CreateMap<StudentApplication, CreateStudentApplicationDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<StudentApplication, UpdateStudentApplicationDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<StudentApplication, StudentApplicationDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
    
            CreateMap<VolunteerOpportunity, CreateVolunteerOpportunityDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<VolunteerOpportunity, UpdateVolunteerOpportunityDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<VolunteerOpportunity, VolunteerOpportunityDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));

            CreateMap<VolunteerProgram, CreateVolunteerProgramDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<VolunteerProgram, CreateInternalVolunteerProgramDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<VolunteerProgram, UpdateVolunteerProgramDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<VolunteerProgram, UpdateInternalVolunteerProgramDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<VolunteerProgram, VolunteerProgramDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));

            CreateMap<VolunteerProgramActivity, CreateVolunteerProgramActivityDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<VolunteerProgramActivity, UpdateVolunteerProgramActivityDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<VolunteerProgramActivity, VolunteerProgramActivityDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));

            CreateMap<VolunteerProgramGalleryPhoto, CreateGalleryPhotoDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<VolunteerProgramGalleryPhoto, UpdateGalleryPhotoDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<VolunteerProgramGalleryPhoto, CreateGalleryPhotoByStudentDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<VolunteerProgramGalleryPhoto, UpdateGalleryPhotoByStudentDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<VolunteerProgramGalleryPhoto, VolunteerProgramGalleryPhotoDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));

            CreateMap<VolunteerProgramTask, CreateVolunteerProgramTaskDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<VolunteerProgramTask, UpdateVolunteerProgramTaskDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<VolunteerProgramTask, VolunteerProgramTaskDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));

            CreateMap<VolunteerProgramWorkDay, CreateVolunteerProgramWorkDayDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<VolunteerProgramWorkDay, UpdateVolunteerProgramWorkDayDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<VolunteerProgramWorkDay, VolunteerProgramWorkDayDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));

            CreateMap<VolunteerStudent, CreateVolunteerStudentDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<VolunteerStudent, UpdateVolunteerStudentOrgAssessmentDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<VolunteerStudent, UpdateVolunteerStudentFinalEvaluationDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<VolunteerStudent, VolunteerStudentDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));

            CreateMap<VolunteerStudentActivityAttendance, CreateVolunteerStudentActivityAttendanceDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<VolunteerStudentActivityAttendance, UpdateVolunteerStudentActivityAttendanceDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<VolunteerStudentActivityAttendance, VolunteerStudentActivityAttendanceDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));

            CreateMap<VolunteerStudentTaskAccomplish, CreateVolunteerStudentTaskAccomplishDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<VolunteerStudentTaskAccomplish, UpdateVolunteerStudentTaskAccomplishDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<VolunteerStudentTaskAccomplish, VolunteerStudentTaskAccomplishDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));

            CreateMap<VolunteerStudentWorkAttendance, CreateVolunteerStudentWorkAttendanceDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<VolunteerStudentWorkAttendance, UpdateVolunteerStudentWorkAttendanceDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<VolunteerStudentWorkAttendance, VolunteerStudentWorkAttendanceDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
        }
    }
}
