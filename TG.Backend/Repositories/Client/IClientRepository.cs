using TG.Backend.Models.Client;

namespace TG.Backend.Repositories.Client
{
    public interface IClientRepository
    {
        Task CreateClient(CreateClientDTO dto);
        Task<bool> DeleteClient(DeleteClientDTO dto);
        Task<ClientResponseDTO?> GetClient(ClientDTO dto);
        Task<IEnumerable<ClientResponseDTO>> GetClients();
    }
}