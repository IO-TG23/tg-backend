namespace TG.Backend.Services
{
    public class DevSendPasswordTokenService : ISendPasswordTokenService
    {
        private readonly ILogger<DevSendPasswordTokenService> _logger;

        public DevSendPasswordTokenService(ILogger<DevSendPasswordTokenService> logger)
        {
            _logger = logger;
        }

        public Task SendToken(AppUser user, string token)
        {
            _logger.LogInformation("Token: {token} for user: {user}", token, user.Email);
            return Task.CompletedTask;
        }
    }
}
