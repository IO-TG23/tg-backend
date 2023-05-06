using TG.Backend.Features.Client.Command;
using TG.Backend.Models.Client;
using TG.Backend.Repositories.Client;

namespace TG.Backend.Features.Client.Handler
{
    public class GetClientsHandler : IRequestHandler<GetClientsQuery, ClientResponseModel>
    {
        private readonly IClientRepository _clientRepository;

        public GetClientsHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<ClientResponseModel> Handle(GetClientsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<ClientResponseDTO> clients = await _clientRepository.GetClients();

            if (clients is null || clients.Count() == 0)
            {
                return new()
                {
                    IsSuccess = false,
                    Messages = new[] { "No clients present" },
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            return new()
            {
                IsSuccess = true,
                Messages = Array.Empty<string>(),
                StatusCode = HttpStatusCode.OK,
                Clients = clients
            };
        }
    }
}
