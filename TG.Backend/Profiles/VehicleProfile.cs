using AutoMapper;
using TG.Backend.Helpers;

namespace TG.Backend.Profiles;

public class VehicleProfile : Profile
{
    public VehicleProfile()
    {
        CreateMap<Vehicle, VehicleDTO>()
            .ForMember(v => v.Gearbox,
                c => c.MapFrom(vehicle => vehicle.Gearbox.ToString()))
            .ForMember(v => v.Drive,
                c => c.MapFrom(vehicle => vehicle.Drive.ToString()));

        CreateMap<VehicleDTO, Vehicle>()
            .ForMember(v => v.Gearbox,
                c => c.MapFrom(vehicle => vehicle.Gearbox.GetGearbox()))
            .ForMember(v => v.Drive,
                c => c.MapFrom(vehicle => vehicle.Drive.GetDrive()));
    }
}