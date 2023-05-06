using TG.Backend.Features.Client.Command;
using TG.Backend.Models.Client;
using TG.Backend.Repositories.Client;

namespace TG.Backend.Features.Client.Handler
{
    public class DeleteClientHandler : IRequestHandler<DeleteClientCommand, ClientResponseModel>
    {
        private readonly IClientRepository _clientRepository;

        public DeleteClientHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<ClientResponseModel> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
        {
            bool deleteRes = await _clientRepository.DeleteClient(request.Client);

            return deleteRes ? new()
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.NoContent,
                Messages = Array.Empty<string>()
            } : new()
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound,
                Messages = new[] { "Client with the given ID not found" }
            };
        }
    }
}
