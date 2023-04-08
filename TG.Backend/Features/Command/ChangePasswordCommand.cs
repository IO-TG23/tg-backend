using MediatR;

namespace TG.Backend.Features.Command
{
    public record ChangePasswordCommand(AppUserChangePasswordDTO AppUser) : IRequest<AuthResponseModel> { }
}
