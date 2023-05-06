namespace TG.Backend.Features.Command
{
    public record AssignUserToRoleCommand(AssignUserToRoleDTO Assignment) : IRequest<AuthResponseModel> { }
}
