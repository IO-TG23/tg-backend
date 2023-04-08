using AutoMapper;

namespace TG.Backend.Profiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<AppUserRegisterDTO, AppUser>();
            CreateMap<AppUserLoginDTO, AppUser>();
        }
    }
}
