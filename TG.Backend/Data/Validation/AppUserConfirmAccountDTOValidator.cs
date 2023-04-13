using FluentValidation;

namespace TG.Backend.Data.Validation
{
    public class AppUserConfirmAccountDTOValidator : AbstractValidator<AppUserConfirmAccountDTO>
    {
        public AppUserConfirmAccountDTOValidator()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
            RuleFor(x => x.Token).NotEmpty();
        }
    }
}
