using AutoMapper;

namespace TG.Backend.Features.Vehicle.GetVehicles;

public class GetVehiclesQueryHandler : IRequestHandler<GetVehiclesQuery, VehicleResponse>
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IMapper _mapper;

    public GetVehiclesQueryHandler(IVehicleRepository vehicleRepository, IMapper mapper)
    {
        _vehicleRepository = vehicleRepository;
        _mapper = mapper;
    }

    public async Task<VehicleResponse> Handle(GetVehiclesQuery request, CancellationToken cancellationToken)
    {
        var vehicles = await _vehicleRepository.GetVehiclesAsync();

        return new VehicleResponse
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Vehicles = _mapper.Map<IEnumerable<VehicleDTO>>(vehicles)
        };
    }
}