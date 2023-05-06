using TG.Backend.Models.Client;

namespace TG.Backend.Features.Client.Command
{
    public record GetClientsQuery : IRequest<ClientResponseModel>;
}
