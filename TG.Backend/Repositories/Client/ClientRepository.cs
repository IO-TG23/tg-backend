using AutoMapper;
using TG.Backend.Models.Client;

namespace TG.Backend.Repositories.Client
{
    public class ClientRepository : IClientRepository
    {
        private readonly AppDbContext _ctx;
        private readonly IMapper _mapper;

        public ClientRepository(AppDbContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<ClientResponseDTO?> GetClient(ClientDTO dto)
        {
            ClientResponseDTO? c = await _ctx.Clients.Include(c => c.AppUser)
                .Select(c => new ClientResponseDTO() { Id = c.Id, Email = c.AppUser.Email })
                .FirstOrDefaultAsync(c => c.Id == dto.Id);

            return c;
        }

        public async Task<IEnumerable<ClientResponseDTO>> GetClients()
        {
            IEnumerable<ClientResponseDTO> clients = await _ctx.Clients.Include(c => c.AppUser)
                .Select(c => new ClientResponseDTO() { Id = c.Id, Email = c.AppUser.Email })
                .ToListAsync();

            return clients;
        }

        public async Task CreateClient(CreateClientDTO dto)
        {
            Data.Client client = _mapper.Map<Data.Client>(dto);

            await _ctx.Clients.AddAsync(client);

            await _ctx.SaveChangesAsync();
        }

        public async Task<bool> DeleteClient(DeleteClientDTO dto)
        {
            Data.Client? c = _ctx.Clients.FirstOrDefault(c => c.Id == dto.Id);

            if (c is null)
            {
                return false;
            }

            _ctx.Clients.Remove(c);

            await _ctx.SaveChangesAsync();

            return true;
        }
    }
}
