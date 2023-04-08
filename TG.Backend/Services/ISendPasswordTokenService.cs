namespace TG.Backend.Services
{
    public interface ISendPasswordTokenService
    {
        Task SendToken(AppUser user, string token);
    }
}
