using Microsoft.AspNetCore.Identity.UI.Services;

namespace TG.Backend.Services
{
    public class ProdSendPasswordTokenService : ISendPasswordTokenService
    {
        private readonly IEmailSender _sender;

        public ProdSendPasswordTokenService(IEmailSender sender)
        {
            _sender = sender;
        }

        public async Task SendToken(AppUser user, string token)
        {
            await _sender.SendEmailAsync(user.Email, "Your account token", token);
        }
    }
}
