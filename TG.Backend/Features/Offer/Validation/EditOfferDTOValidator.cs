using FluentValidation;
using TG.Backend.Features.Offer.CreateOffer;
using TG.Backend.Features.Offer.EditOffer;
using TG.Backend.Models.Offer;

namespace TG.Backend.Features.Offer.Validation;

public class EditOfferDTOValidator : AbstractValidator<EditOfferCommand>
{
    public EditOfferDTOValidator()
    {
        RuleFor(e => e.Id)
            .NotEmpty()
            .WithMessage("Id of offer cannot be empty!");

        RuleFor(e => e.EditOfferDto.OfferDto)
            .NotNull()
            .WithMessage("Offer cannot be null!");

        RuleFor(e => new CreateOfferCommand(e.EditOfferDto.OfferDto))
            .SetValidator(new CreateOfferDTOValidator());
    }
}