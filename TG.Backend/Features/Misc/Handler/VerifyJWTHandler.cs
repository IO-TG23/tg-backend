using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TG.Backend.Features.Misc.Command;

namespace TG.Backend.Features.Misc.Handler
{
    public class VerifyJWTHandler : IRequestHandler<VerifyJWTQuery, bool>
    {
        private readonly IConfiguration _configuration;

        public VerifyJWTHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<bool> Handle(VerifyJWTQuery request, CancellationToken cancellationToken)
        {
            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"])),
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"]
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            try
            {
                handler.ValidateToken(request.Verification.Token, validationParameters, out var validToken);
                JwtSecurityToken validJwt = validToken as JwtSecurityToken;
                return Task.FromResult(validJwt is not null);
            }
            catch
            {
                return Task.FromResult(false);
            }

        }
    }
}
