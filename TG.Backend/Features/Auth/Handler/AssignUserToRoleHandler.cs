namespace TG.Backend.Features.Auth.Handler
{
    public class AssignUserToRoleHandler : IRequestHandler<AssignUserToRoleCommand, AuthResponseModel>
    {
        private readonly UserManager<AppUser> _userManager;

        public AssignUserToRoleHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AuthResponseModel> Handle(AssignUserToRoleCommand request, CancellationToken cancellationToken)
        {
            AppUser? user = await _userManager.FindByEmailAsync(request.Assignment.Email);

            if (user is null)
            {
                return new()
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Messages = new[] { "User not found" }
                };
            }

            IdentityResult res = await _userManager.AddToRoleAsync(user, request.Assignment.Role);

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
                StatusCode = HttpStatusCode.OK,
                Messages = new[] { "Assignment to new role successful" }
            };
        }
    }
}
