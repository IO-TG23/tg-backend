using FluentValidation;

namespace TG.Backend.Data.Validation
{
    public class AppUserResetPasswordDTOValidator : AbstractValidator<AppUserResetPasswordDTO>
    {
        public AppUserResetPasswordDTOValidator()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
        }
    }
}
