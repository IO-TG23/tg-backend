using FluentValidation;

namespace TG.Backend.Models.Auth.Validation
{
    public class AppUserDeleteDTOValidator : AbstractValidator<AppUserDeleteDTO>
    {
        public AppUserDeleteDTOValidator()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
        }
    }
}
