using FluentValidation;

namespace TG.Backend.Data.Validation
{
    public class AppUserChangePasswordDTOValidator : AbstractValidator<AppUserChangePasswordDTO>
    {
        public AppUserChangePasswordDTOValidator()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
            RuleFor(x => x.NewPassword).NotEmpty();
            RuleFor(x => x.Token).NotEmpty();
        }
    }
}
