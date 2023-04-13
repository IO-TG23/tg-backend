using AutoMapper;

namespace TG.Backend.Features.Vehicle.CreateVehicle;

public class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, VehicleResponse>
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IMapper _mapper;

    public CreateVehicleCommandHandler(IVehicleRepository vehicleRepository, IMapper mapper)
    {
        _vehicleRepository = vehicleRepository;
        _mapper = mapper;
    }

    public async Task<VehicleResponse> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        var vehicle = _mapper.Map<Data.Vehicle>(request.VehicleDto);

        await _vehicleRepository.CreateVehicleAsync(vehicle);

        return new VehicleResponse
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.NoContent
        };
    }
}