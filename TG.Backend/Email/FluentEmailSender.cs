using FluentEmail.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;

namespace TG.Backend.Email
{
    /// <summary>
    /// Produkcyjna implementacja email sendera - wykorzystuje EmailSender
    /// </summary>
    public class FluentEmailSender : IEmailSender
    {
        private readonly string email;

        public FluentEmailSender(IConfiguration configuration)
        {
            email = configuration["Mailing:Email"];

            SmtpSender sender = new(new SmtpClient()
            {
                EnableSsl = true,
                Port = 587,
                Credentials = new NetworkCredential(email, configuration["Mailing:Password"])
            });

            FluentEmail.Core.Email.DefaultSender = sender;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            await FluentEmail.Core.Email.From(email).Subject(subject).Body(htmlMessage).SendAsync();
        }
    }
}
