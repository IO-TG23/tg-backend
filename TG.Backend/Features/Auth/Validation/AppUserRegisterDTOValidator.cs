using FluentValidation;

namespace TG.Backend.Features.Auth.Validation
{
    public class AppUserRegisterDTOValidator : AbstractValidator<RegisterUserCommand>
    {
        public AppUserRegisterDTOValidator()
        {
            RuleFor(x => x.AppUser.Email)
                .EmailAddress()
                .NotEmpty();
            
            RuleFor(x => x.AppUser.Password)
                .NotEmpty();
            
            RuleFor(x => x.AppUser.ConfirmPassword)
                .Equal(x => x.AppUser.Password);
        }
    }
}
