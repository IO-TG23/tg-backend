using MediatR;

namespace TG.Backend.Features.Command
{
    public record DeleteUserCommand(AppUserDeleteDTO AppUser) : IRequest<AuthResponseModel> { }
}
