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
            .GreaterThanOrEqualTo(1900)
            .WithMessage("Production start year must be greater or equal to 1900!");

        RuleFor(v => v.CreateOfferDto.Vehicle.ProductionEndYear)
            .Must(v => v is null or > 1900)
            .WithMessage("Production end year must be empty or greater than 1900!")
            .GreaterThanOrEqualTo(command => command.CreateOfferDto.Vehicle.ProductionStartYear)
            .When(command => command.CreateOfferDto.Vehicle.ProductionEndYear > 1900, ApplyConditionTo.CurrentValidator)
            .WithMessage("Production end year must be greater or equal to production start year!");

        RuleFor(v => v.CreateOfferDto.Vehicle.NumberOfDoors)
            .GreaterThan(0)
            .WithMessage("Number of doors must be positive");

        RuleFor(v => v.CreateOfferDto.Vehicle.NumberOfSeats)
            .GreaterThan(0)
            .WithMessage("Number of seats must be positive");

        RuleFor(v => v.CreateOfferDto.Vehicle.BootCapacity)
            .GreaterThan(0)
            .WithMessage("Boot capacity must be positive");

        RuleFor(v => v.CreateOfferDto.Vehicle.Length)
            .GreaterThan(0)
            .WithMessage("Length of vehicle must be positive!");

        RuleFor(v => v.CreateOfferDto.Vehicle.Height)
            .GreaterThan(0)
            .WithMessage("Height of vehicle must be positive!");

        RuleFor(v => v.CreateOfferDto.Vehicle.Width)
            .GreaterThan(0)
            .WithMessage("Width of vehicle must be positive!");

        RuleFor(v => v.CreateOfferDto.Vehicle.WheelBase)
            .GreaterThan(0)
            .WithMessage("Wheel base of vehicle must be positive!");

        RuleFor(v => v.CreateOfferDto.Vehicle.BackWheelTrack)
            .GreaterThan(0)
            .WithMessage("Back wheel track of vehicle must be positive!");

        RuleFor(v => v.CreateOfferDto.Vehicle.FrontWheelTrack)
            .GreaterThan(0)
            .WithMessage("Front wheel track of vehicle must be positive!");

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
            .GreaterThan(0)
            .WithMessage("Price must be positive!");

        RuleFor(v => v.CreateOfferDto.ContactEmail)
            .NotEmpty()
            .WithMessage("Contact email cannot be empty!")
            .EmailAddress()
            .When(c => !string.IsNullOrWhiteSpace(c.CreateOfferDto.ContactEmail), ApplyConditionTo.CurrentValidator)
            .WithMessage("This is not valid email address!");

        RuleFor(v => v.CreateOfferDto.ContactPhoneNumber)
            .NotEmpty()
            .WithMessage("Contact phone number cannot be empty!")
            .Matches(@"^\d{9}$")
            .When(c => !string.IsNullOrWhiteSpace(c.CreateOfferDto.ContactPhoneNumber), ApplyConditionTo.CurrentValidator)
            .WithMessage("Invalid phone number!");
    }
}