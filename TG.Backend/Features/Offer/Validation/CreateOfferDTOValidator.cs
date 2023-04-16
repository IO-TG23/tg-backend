using FluentValidation;
using TG.Backend.Features.Offer.CreateOffer;
using TG.Backend.Helpers;

namespace TG.Backend.Features.Offer.Validation;

public class CreateOfferDTOValidator : AbstractValidator<CreateOfferCommand>
{
    public CreateOfferDTOValidator()
    {
        RuleFor(v => v.CreateOfferDto.Vehicle.Name)
            .NotEmpty()
            .WithMessage("Name of vehicle cannot be empty!");
        
        RuleFor(v => v.CreateOfferDto.Vehicle.Description)
            .NotEmpty()
            .WithMessage("Description of vehicle cannot be empty!");

        RuleFor(v => v.CreateOfferDto.Vehicle.ProductionStartYear)
            .NotEmpty()
            .WithMessage("Product start year cannot be empty!")
            .GreaterThanOrEqualTo(1900)
            .WithMessage("Production start year must be greater or equal to 1900!");

        RuleFor(v => v.CreateOfferDto.Vehicle.ProductionEndYear)
            .Must(BeValidProductionEndYear)
            .WithMessage("Production end year must be empty or greater than 1900!")
            .GreaterThanOrEqualTo(command => command.CreateOfferDto.Vehicle.ProductionStartYear)
            .WithMessage("Production end year must be greater or equal to production start year!");

        RuleFor(v => v.CreateOfferDto.Vehicle.NumberOfDoors)
            .NotEmpty()
            .WithMessage("Number of doors  cannot be empty!");

        RuleFor(v => v.CreateOfferDto.Vehicle.NumberOfSeats)
            .NotEmpty()
            .WithMessage("Number of seats cannot be empty!");

        RuleFor(v => v.CreateOfferDto.Vehicle.BootCapacity)
            .NotEmpty()
            .WithMessage("Boot capacity cannot be empty!");

        RuleFor(v => v.CreateOfferDto.Vehicle.Length)
            .NotEmpty()
            .WithMessage("Length of vehicle cannot be empty!")
            .GreaterThan(0)
            .WithMessage("Length of vehicle cannot be negative!");
        
        RuleFor(v => v.CreateOfferDto.Vehicle.Height)
            .NotEmpty()
            .WithMessage("Height of vehicle cannot be empty!")
            .GreaterThan(0)
            .WithMessage("Height of vehicle cannot be negative!");
        
        RuleFor(v => v.CreateOfferDto.Vehicle.Width)
            .NotEmpty()
            .WithMessage("Width of vehicle cannot be empty!")
            .GreaterThan(0)
            .WithMessage("Width of vehicle cannot be negative!");
        
        RuleFor(v => v.CreateOfferDto.Vehicle.WheelBase)
            .NotEmpty()
            .WithMessage("Wheel base of vehicle cannot be empty!")
            .GreaterThan(0)
            .WithMessage("Wheel base of vehicle cannot be negative!");
        
        RuleFor(v => v.CreateOfferDto.Vehicle.BackWheelTrack)
            .NotEmpty()
            .WithMessage("Back wheel track of vehicle cannot be empty!")
            .GreaterThan(0)
            .WithMessage("Back wheel track of vehicle cannot be negative!");
        
        RuleFor(v => v.CreateOfferDto.Vehicle.FrontWheelTrack)
            .NotEmpty()
            .WithMessage("Front wheel track of vehicle cannot be empty!")
            .GreaterThan(0)
            .WithMessage("Front wheel track of vehicle cannot be negative!");

        RuleFor(v => v.CreateOfferDto.Vehicle.Gearbox)
            .NotEmpty()
            .WithMessage("Gearbox cannot be empty!")
            .Must(e => e.BeValidGearbox())
            .WithMessage(@"Valid values for gearbox are: ""Automatic"" or ""Manual""!");
        
        RuleFor(v => v.CreateOfferDto.Vehicle.Drive)
            .NotEmpty()
            .WithMessage("Vehicle drive cannot be empty!")
            .Must(e => e.BeValidDrive())
            .WithMessage(@"Valid values for gearbox are: ""AWD"", ""RWD"" or ""FWD""!");

        RuleFor(v => v.CreateOfferDto.Description)
            .NotEmpty()
            .WithMessage("Offer description cannot be empty!");

        RuleFor(v => v.CreateOfferDto.Price)
            .NotEmpty()
            .WithMessage("Price cannot be empty!")
            .GreaterThan(0)
            .WithMessage("Price cannot be negative!");

        RuleFor(v => v.CreateOfferDto.ContactEmail)
            .NotEmpty()
            .WithMessage("Contact email cannot be empty!")
            .EmailAddress()
            .WithMessage("This is not valid email address!");

        RuleFor(v => v.CreateOfferDto.ContactPhoneNumber)
            .NotEmpty()
            .WithMessage("Contact phone number cannot be empty!")
            .Matches(@"^\d{9}$")
            .WithMessage("Invalid phone number!");
    }
    
    private static bool BeValidProductionEndYear(int? value)
    {
        if (value is null) return true;

        return value >= 1901;
    }
}