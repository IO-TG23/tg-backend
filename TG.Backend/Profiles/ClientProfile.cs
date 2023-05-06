using AutoMapper;
using TG.Backend.Models.Client;

namespace TG.Backend.Profiles
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<CreateClientDTO, Client>();
        }
    }
}
