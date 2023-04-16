using MediatR;

namespace TG.Backend.Features.Command
{
    public record RegisterUserCommand(AppUserRegisterDTO AppUser) : IRequest<AuthResponseModel> { }
}
