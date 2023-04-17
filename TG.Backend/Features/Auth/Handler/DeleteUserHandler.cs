namespace TG.Backend.Features.Handler
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, AuthResponseModel>
    {
        private readonly UserManager<AppUser> _userManager;

        public DeleteUserHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AuthResponseModel> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
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

            IdentityResult res = await _userManager.DeleteAsync(user);

            if (!res.Succeeded)
            {
                return new()
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.Unauthorized,
                    Messages = res.Errors.Select(e => e.Description).ToArray()
                };
            }

            return new()
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.NoContent,
                Messages = new[] { $"User successfully deleted" }
            };
        }
    }
}
