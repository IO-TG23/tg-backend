using FluentValidation;

namespace TG.Backend.Data.Validation
{
    public class AppUserDeleteDTOValidator : AbstractValidator<AppUserDeleteDTO>
    {
        public AppUserDeleteDTOValidator()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
        }
    }
}
