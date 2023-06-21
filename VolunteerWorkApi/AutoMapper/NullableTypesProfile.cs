using AutoMapper;
using VolunteerWorkApi.Enums;

namespace VolunteerWorkApi.AutoMapper
{
    public class NullableTypesProfile : Profile
    {
        public NullableTypesProfile()
        {
            CreateMap<bool?, bool>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<int?, int>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<short?, short>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<byte?, byte>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<decimal?, decimal>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<long?, long>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<double?, double>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<float?, float>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<char?, char>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<DateTime?, DateTime>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<string?, string>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<ApplicationStatus?, ApplicationStatus>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<NotificationPage?, NotificationPage>().ConvertUsing((src, dest) => src ?? dest);
        }
    }
}
