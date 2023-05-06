using TG.Backend.Models.Client;

namespace TG.Backend.Features.Client.Command
{
    public record GetClientQuery(ClientDTO Client) : IRequest<ClientResponseModel>;
}
