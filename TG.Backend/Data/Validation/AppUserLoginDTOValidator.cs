using FluentValidation;

namespace TG.Backend.Data.Validation
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
