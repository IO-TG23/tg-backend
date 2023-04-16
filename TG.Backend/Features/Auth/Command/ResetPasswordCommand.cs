using MediatR;

namespace TG.Backend.Features.Command
{
    public record ResetPasswordCommand(AppUserResetPasswordDTO AppUser) : IRequest<AuthResponseModel> { }
}
