using FluentValidation;

namespace TG.Backend.Features.Auth.Validation
{
    public class AppUserLoginDTOValidator : AbstractValidator<LoginUserCommand>
    {
        public AppUserLoginDTOValidator()
        {
            RuleFor(x => x.AppUser.Email)
                .EmailAddress()
                .NotEmpty();
            
            RuleFor(x => x.AppUser.Password)
                .NotEmpty();
        }
    }
}
