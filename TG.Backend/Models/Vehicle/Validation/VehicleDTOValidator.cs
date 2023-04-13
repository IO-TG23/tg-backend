using FluentValidation;
using TG.Backend.Features.Vehicle.CreateVehicle;

namespace TG.Backend.Models.Vehicle.Validation;

public class VehicleDTOValidator : AbstractValidator<CreateVehicleCommand>
{
    public VehicleDTOValidator()
    {
        RuleFor(v => v.VehicleDto.Name)
            .NotEmpty()
            .WithMessage("Name of vehicle cannot be empty!");

        RuleFor(v => v.VehicleDto.ProductionStartYear)
            .NotEmpty()
            .WithMessage("Product start year cannot be empty!")
            .GreaterThanOrEqualTo(1900)
            .WithMessage("Production start year must be greater or equal to 1900!");

        RuleFor(v => v.VehicleDto.ProductionEndYear)
            .Must(BeValidProductionEndYear)
            .WithMessage("Production end year must be empty or greater than 1900!")
            .GreaterThanOrEqualTo(command => command.VehicleDto.ProductionStartYear)
            .WithMessage("Production end year must be greater or equal to production start year!");

        RuleFor(v => v.VehicleDto.NumberOfDoors)
            .NotEmpty()
            .WithMessage("Number of doors  cannot be empty!");

        RuleFor(v => v.VehicleDto.NumberOfSeats)
            .NotEmpty()
            .WithMessage("Number of seats cannot be empty!");

        RuleFor(v => v.VehicleDto.BootCapacity)
            .NotEmpty()
            .WithMessage("Boot capacity cannot be empty!");

        RuleFor(v => v.VehicleDto.Length)
            .NotEmpty()
            .WithMessage("Length of vehicle cannot be empty!")
            .GreaterThan(0)
            .WithMessage("Length of vehicle cannot be negative!");
        
        RuleFor(v => v.VehicleDto.Height)
            .NotEmpty()
            .WithMessage("Height of vehicle cannot be empty!")
            .GreaterThan(0)
            .WithMessage("Height of vehicle cannot be negative!");
        
        RuleFor(v => v.VehicleDto.Width)
            .NotEmpty()
            .WithMessage("Width of vehicle cannot be empty!")
            .GreaterThan(0)
            .WithMessage("Width of vehicle cannot be negative!");
        
        RuleFor(v => v.VehicleDto.WheelBase)
            .NotEmpty()
            .WithMessage("Wheel base of vehicle cannot be empty!")
            .GreaterThan(0)
            .WithMessage("Wheel base of vehicle cannot be negative!");
        
        RuleFor(v => v.VehicleDto.BackWheelTrack)
            .NotEmpty()
            .WithMessage("Back wheel track of vehicle cannot be empty!")
            .GreaterThan(0)
            .WithMessage("Back wheel track of vehicle cannot be negative!");
        
        RuleFor(v => v.VehicleDto.FrontWheelTrack)
            .NotEmpty()
            .WithMessage("Front wheel track of vehicle cannot be empty!")
            .GreaterThan(0)
            .WithMessage("Front wheel track of vehicle cannot be negative!");

        RuleFor(v => v.VehicleDto.Gearbox)
            .NotEmpty()
            .WithMessage("Gearbox cannot be empty!")
            .Must(BeValidGearbox)
            .WithMessage(@"Valid values for gearbox are: ""Automatic"" or ""Manual""!");
        
        RuleFor(v => v.VehicleDto.Drive)
            .NotEmpty()
            .WithMessage("Vehicle drive cannot be empty!")
            .Must(BeValidDrive)
            .WithMessage(@"Valid values for gearbox are: ""AWD"", ""RWD"" or ""FWD""!");
    }
    
    private static bool BeValidGearbox(string value)
    {
        return Enum.TryParse<Gearbox>(value, out var gearbox);
    }

    private static bool BeValidDrive(string value)
    {
        return Enum.TryParse<Drive>(value, out var drive);
    }

    private static bool BeValidProductionEndYear(int? value)
    {
        if (value is null) return true;

        return value >= 1901;
    }
}