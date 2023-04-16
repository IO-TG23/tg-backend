using FluentValidation;

namespace TG.Backend.Features.Auth.Validation
{
    public class AppUserConfirmAccountDTOValidator : AbstractValidator<ConfirmAccountCommand>
    {
        public AppUserConfirmAccountDTOValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .NotEmpty();
            
            RuleFor(x => x.Token)
                .NotEmpty();
        }
    }
}
