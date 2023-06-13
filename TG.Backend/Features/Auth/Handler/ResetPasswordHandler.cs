using System.Web;
using TG.Backend.Services;

namespace TG.Backend.Features.Handler
{
    public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, AuthResponseModel>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ISendPasswordTokenService _sendPasswordTokenService;
        private readonly IConfiguration _configuration;

        public ResetPasswordHandler(UserManager<AppUser> userManager, ISendPasswordTokenService sendPasswordTokenService,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _sendPasswordTokenService = sendPasswordTokenService;
            _configuration = configuration;
        }

        public async Task<AuthResponseModel> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            AppUser user = await _userManager.FindByEmailAsync(request.AppUser.Email);

            if (user is null)
            {
                return new()
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.Unauthorized,
                    Messages = new[] { "Provided credentials are invalid" }
                };
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            string emailMessage = $"<div>Twój token resetujący hasło: <b>{HttpUtility.UrlEncode(token)}</b> <br /><br /></div>";

            await _sendPasswordTokenService.SendToken(user, emailMessage);

            return new()
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.NoContent,
                Messages = new string[] { }
            };
        }
    }
}
