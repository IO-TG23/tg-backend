namespace TG.Backend.Features.Command
{
    public record RemoveUserFromRoleCommand(RemoveUserFromRoleDTO Removal) : IRequest<AuthResponseModel> { }
}
