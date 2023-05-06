using TG.Backend.Models;

namespace TG.Backend.Features.Misc.Command
{
    public record VerifyJWTQuery(JwtTokenVerification Verification) : IRequest<bool>;
}
