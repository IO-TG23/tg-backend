using FluentValidation;

namespace TG.Backend.Features.Auth.Validation
{
    public class AppUserDeleteDTOValidator : AbstractValidator<DeleteUserCommand>
    {
        public AppUserDeleteDTOValidator()
        {
            RuleFor(x => x.AppUser.Email)
                .EmailAddress()
                .NotEmpty();
        }
    }
}
