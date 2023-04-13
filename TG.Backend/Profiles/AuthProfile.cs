using AutoMapper;

namespace TG.Backend.Profiles
{
    /// <summary>
    /// Profil dla modeli powiazanych z Auth
    /// </summary>
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<AppUserRegisterDTO, AppUser>();
            CreateMap<AppUserLoginDTO, AppUser>();
            CreateMap<AppUserDeleteDTO, AppUser>();
        }
    }
}
