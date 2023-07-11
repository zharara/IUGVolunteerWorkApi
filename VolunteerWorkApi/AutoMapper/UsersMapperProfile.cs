using AutoMapper;
using VolunteerWorkApi.Dtos.ApplicationUser;
using VolunteerWorkApi.Dtos.ManagementEmployee;
using VolunteerWorkApi.Dtos.Organization;
using VolunteerWorkApi.Dtos.Student;

namespace VolunteerWorkApi.AutoMapper
{
    public class UsersMapperProfile : Profile
    {
        public UsersMapperProfile()
        {
            CreateMap<ApplicationUser, ApplicationUserDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));

            CreateMap<ManagementEmployee, CreateManagementEmployeeDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<ManagementEmployee, UpdateManagementEmployeeDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));   
            CreateMap<ManagementEmployee, ManagementEmployeeDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));

            CreateMap<Organization, CreateOrganizationDto>()
                .ForMember(dist => dist.OrgNameLocal, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dist => dist.OrgNameForeign, opt => opt.MapFrom(src => src.LastName))
                .ReverseMap()
                .ForMember(dist => dist.FirstName, opt => opt.MapFrom(src => src.OrgNameLocal))
                .ForMember(dist => dist.LastName, opt => opt.MapFrom(src => src.OrgNameForeign))
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<Organization, UpdateOrganizationDto>()
                .ForMember(dist => dist.OrgNameLocal, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dist => dist.OrgNameForeign, opt => opt.MapFrom(src => src.LastName))
                .ReverseMap()
                .ForMember(dist => dist.FirstName, opt => opt.MapFrom(src => src.OrgNameLocal))
                .ForMember(dist => dist.LastName, opt => opt.MapFrom(src => src.OrgNameForeign))
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<Organization, OrganizationDto>()
                .ForMember(dist => dist.OrgNameLocal, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dist => dist.OrgNameForeign, opt => opt.MapFrom(src => src.LastName))
                .ReverseMap()
                .ForMember(dist => dist.FirstName, opt => opt.MapFrom(src => src.OrgNameLocal))
                .ForMember(dist => dist.LastName, opt => opt.MapFrom(src => src.OrgNameForeign))
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));

            CreateMap<Student, CreateStudentDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<Student, UpdateStudentDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<Student, UpdateStudentByManagementDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<Student, StudentDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));

        }
    }
}
