using FluentValidation;

namespace TG.Backend.Models.Auth.Validation
{
    public class AppUserResetPasswordDTOValidator : AbstractValidator<AppUserResetPasswordDTO>
    {
        public AppUserResetPasswordDTOValidator()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
        }
    }
}
