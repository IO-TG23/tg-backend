using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace TG.Backend.Email
{
    public class DevEmailSender : IEmailSender
    {
        private readonly UserManager<AppUser> _userManager;

        public DevEmailSender(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            if (subject.StartsWith("Potwierd"))
            {
                AppUser user = await _userManager.FindByEmailAsync(email);

                if (user is not null)
                {
                    user.EmailConfirmed = true;

                    await _userManager.UpdateAsync(user);
                }
            }
        }
    }
}
