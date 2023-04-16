using MediatR;

namespace TG.Backend.Features.Command
{
    public record ConfirmAccountCommand(string Token, string Email) : IRequest<AuthResponseModel> { }
}
