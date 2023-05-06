using TG.Backend.Features.Client.Command;
using TG.Backend.Models.Client;
using TG.Backend.Repositories.Client;

namespace TG.Backend.Features.Client.Handler
{
    public class GetClientHandler : IRequestHandler<GetClientQuery, ClientResponseModel>
    {
        private readonly IClientRepository _clientRepository;

        public GetClientHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<ClientResponseModel> Handle(GetClientQuery request, CancellationToken cancellationToken)
        {
            ClientResponseDTO? client = await _clientRepository.GetClient(request.Client);

            if (client is null)
            {
                return new()
                {
                    IsSuccess = false,
                    Messages = new[] { "No client with given ID present" },
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            return new()
            {
                IsSuccess = true,
                Messages = Array.Empty<string>(),
                StatusCode = HttpStatusCode.OK,
                Clients = new List<ClientResponseDTO>() { client }
            };
        }
    }
}
