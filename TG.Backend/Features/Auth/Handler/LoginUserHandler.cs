using Google.Authenticator;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TG.Backend.Features.Handler
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, AuthResponseModel>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly TwoFactorAuthenticator _authenticator;
        private readonly AppDbContext _ctx;

        public LoginUserHandler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager, IConfiguration configuration,
            TwoFactorAuthenticator authenticator, AppDbContext ctx)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _authenticator = authenticator;
            _ctx = ctx;
        }

        public async Task<AuthResponseModel> Handle(LoginUserCommand request, CancellationToken cancellationToken)
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

            SignInResult loginRes = await _signInManager.PasswordSignInAsync(user, request.AppUser.Password, false, true);
            bool validTFACode = _authenticator.ValidateTwoFactorPIN(_configuration["TFA:Key"], request.AppUser.Code);

            if (!loginRes.Succeeded || loginRes.IsLockedOut || !validTFACode)
            {
                return new()
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.Unauthorized,
                    Messages = new[] { "Provided credentials are invalid or user is locked" }
                };
            }


            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var client = await _ctx.Clients.FirstOrDefaultAsync(c => c.AppUserId == Guid.Parse(user.Id));

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("ClientId",client?.Id.ToString())
            };

            List<Claim> userClaims = await GetClaims(user);

            claims.AddRange(userClaims);

            JwtSecurityToken token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
              _configuration["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: credentials);

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new()
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Messages = new[] { jwtToken }
            };
        }

        private async Task<List<Claim>> GetClaims(AppUser user)
        {
            List<Claim> claims = new();

            IList<Claim> userClaims = await _userManager.GetClaimsAsync(user);
            IList<string> userRoles = await _userManager.GetRolesAsync(user);

            claims.AddRange(userClaims);

            foreach (string userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));

                IdentityRole? role = await _roleManager.FindByNameAsync(userRole);

                if (role is not null)
                {
                    IList<Claim> roleClaims = await _roleManager.GetClaimsAsync(role);

                    foreach (Claim roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }
            return claims;
        }
    }
}
