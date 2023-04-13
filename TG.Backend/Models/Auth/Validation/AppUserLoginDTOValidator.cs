using FluentValidation;

namespace TG.Backend.Models.Auth.Validation
{
    public class AppUserLoginDTOValidator : AbstractValidator<AppUserLoginDTO>
    {
        public AppUserLoginDTOValidator()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
