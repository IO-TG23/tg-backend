namespace TG.Backend.Features.Auth.Handler
{
    public class RemoveUserFromRoleHandler : IRequestHandler<RemoveUserFromRoleCommand, AuthResponseModel>
    {
        private readonly UserManager<AppUser> _userManager;

        public RemoveUserFromRoleHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AuthResponseModel> Handle(RemoveUserFromRoleCommand request, CancellationToken cancellationToken)
        {
            AppUser? user = await _userManager.FindByEmailAsync(request.Removal.Email);

            if (user is null)
            {
                return new()
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Messages = new[] { "User not found" }
                };
            }

            IdentityResult res = await _userManager.RemoveFromRoleAsync(user, request.Removal.Role);

            if (!res.Succeeded)
            {
                return new()
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Messages = res.Errors.Select(e => e.Description).ToArray()
                };
            }

            return new()
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.NoContent,
                Messages = new[] { "Removal from role successful" }
            };
        }
    }
}
