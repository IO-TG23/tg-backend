namespace TG.Backend.Models.Client
{
    public class ClientResponseModel : ResponseModel
    {
        public IEnumerable<ClientResponseDTO>? Clients { get; set; }
    }
}
