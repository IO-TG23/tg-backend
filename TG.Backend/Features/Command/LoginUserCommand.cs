using MediatR;

namespace TG.Backend.Features.Command
{
    public record LoginUserCommand(AppUserLoginDTO AppUser) : IRequest<AuthResponseModel> { }
}
