using TG.Backend.Models.Client;

namespace TG.Backend.Features.Client.Command
{
    public record DeleteClientCommand(DeleteClientDTO Client) : IRequest<ClientResponseModel>;
}
