using FluentValidation;

namespace TG.Backend.Features.Auth.Validation
{
    public class AppUserResetPasswordDTOValidator : AbstractValidator<ResetPasswordCommand>
    {
        public AppUserResetPasswordDTOValidator()
        {
            RuleFor(x => x.AppUser.Email)
                .EmailAddress()
                .NotEmpty();
        }
    }
}
