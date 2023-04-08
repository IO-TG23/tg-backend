using Microsoft.AspNetCore.Identity.UI.Services;

namespace TG.Backend.Email
{
    /// <summary>
    /// DEV-owa implementacja EmailSendera, wypisuje informacje na konsole (tylko w srodowisku DEV)
    /// </summary>
    public class DevEmailSender : IEmailSender
    {
        private readonly ILogger<DevEmailSender> _logger;

        public DevEmailSender(ILogger<DevEmailSender> logger)
        {
            _logger = logger;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            _logger.LogInformation("Subject: {subject} for {email}\nContent: {message}", subject, email, htmlMessage);

            return Task.CompletedTask;
        }
    }
}
