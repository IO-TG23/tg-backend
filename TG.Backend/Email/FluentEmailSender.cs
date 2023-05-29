using FluentEmail.Core;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace TG.Backend.Email
{
    /// <summary>
    /// Produkcyjna implementacja email sendera - wykorzystuje EmailSender
    /// </summary>
    public class FluentEmailSender : IEmailSender
    {
        private readonly IFluentEmail _email;

        public FluentEmailSender(IFluentEmail email)
        {
            _email = email;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var res = await _email.To(email).Subject(subject).Body(htmlMessage, isHtml: true).SendAsync();
        }
    }
}
