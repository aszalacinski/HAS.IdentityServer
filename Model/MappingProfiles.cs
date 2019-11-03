
using HAS.IdentityServer.Model;
using static HAS.IdentityServer.Data.ProfileContext;
using MapperProfile = AutoMapper.Profile;

namespace HAS.IdentityServer
{
    public class ProfileProfile : MapperProfile
    {
        public ProfileProfile()
        {
            CreateMap<ProfileDAO, Profile>()
                .ForMember(m => m.PersonalDetails, opt => opt.MapFrom(src => src.PersonalDetails))
                .ForMember(m => m.AppDetails, opt => opt.MapFrom(src => src.AppDetails));
            CreateMap<PersonalDetailsDAO, PersonalDetails>()
                .ForMember(m => m.Location, opt => opt.MapFrom(src => src.Location));
            CreateMap<LocationDetailsDAO, LocationDetails>();
            CreateMap<AppDetailsDAO, AppDetails>()
                .ForMember(m => m.Subscriptions, opt => opt.MapFrom(src => src.Subscriptions))
                .ForMember(m => m.InstructorDetails, opt => opt.MapFrom(src => src.InstructorDetails));
            CreateMap<SubscriptionDetailsDAO, SubscriptionDetails>()
                .ForMember(m => m.Classes, opt => opt.MapFrom(src => src.Classes));
            CreateMap<InstructorDetailsDAO, InstructorDetails>();
            CreateMap<ClassDetailsDAO, ClassDetails>();
        }

    }
}
