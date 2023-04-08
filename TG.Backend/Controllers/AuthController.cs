using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TG.Backend.Filters;

namespace TG.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AppUserRegisterDTO appUser)
        {
            AuthResponseModel resp = await _mediator.Send(new RegisterUserCommand(appUser));

            return resp switch
            {
                { IsSuccess: false, StatusCode: HttpStatusCode.BadRequest } => BadRequest(new { resp.Messages }),
                { IsSuccess: true, StatusCode: HttpStatusCode.OK } => Ok(new { Message = resp.Messages.FirstOrDefault() }),
                _ => BadRequest()
            };
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AppUserLoginDTO appUser)
        {
            AuthResponseModel resp = await _mediator.Send(new LoginUserCommand(appUser));

            return resp switch
            {
                { IsSuccess: false, StatusCode: HttpStatusCode.BadRequest } => BadRequest(new { resp.Messages }),
                { IsSuccess: true, StatusCode: HttpStatusCode.OK } => Ok(new { Message = resp.Messages.FirstOrDefault() }),
                _ => BadRequest()
            };
        }

        [HttpDelete("delete")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> Delete([FromBody] AppUserDeleteDTO appUser)
        {
            AuthResponseModel resp = await _mediator.Send(new DeleteUserCommand(appUser));

            return resp switch
            {
                { IsSuccess: false, StatusCode: HttpStatusCode.BadRequest } => BadRequest(new { resp.Messages }),
                { IsSuccess: true, StatusCode: HttpStatusCode.NoContent } => NoContent(),
                _ => BadRequest()
            };
        }

        [HttpPost("resetPassword")]
        [ServiceFilter(typeof(ValidateAccountNotLockedFilter))]
        public async Task<IActionResult> ResetPassword([FromBody] AppUserResetPasswordDTO appUser)
        {
            AuthResponseModel resp = await _mediator.Send(new ResetPasswordCommand(appUser));

            return resp switch
            {
                { IsSuccess: false, StatusCode: HttpStatusCode.BadRequest } => BadRequest(new { resp.Messages }),
                { IsSuccess: true, StatusCode: HttpStatusCode.NoContent } => NoContent(),
                _ => BadRequest()
            };
        }

        [HttpPost("changePassword")]
        [ServiceFilter(typeof(ValidateAccountNotLockedFilter))]
        public async Task<IActionResult> ChangePassword([FromBody] AppUserChangePasswordDTO appUser)
        {
            AuthResponseModel resp = await _mediator.Send(new ChangePasswordCommand(appUser));

            return resp switch
            {
                { IsSuccess: false, StatusCode: HttpStatusCode.BadRequest } => BadRequest(new { resp.Messages }),
                { IsSuccess: true, StatusCode: HttpStatusCode.NoContent } => NoContent(),
                _ => BadRequest()
            };
        }

        [HttpPost("confirmAccount")]
        public async Task<IActionResult> ConfirmAccount([FromBody] AppUserConfirmAccountDTO appUser)
        {
            AuthResponseModel resp = await _mediator.Send(new ConfirmAccountCommand(appUser.Token, appUser.Email));

            return resp switch
            {
                { IsSuccess: false, StatusCode: HttpStatusCode.BadRequest } => BadRequest(new { resp.Messages }),
                { IsSuccess: true, StatusCode: HttpStatusCode.NoContent } => NoContent(),
                _ => BadRequest()
            };
        }
    }
}
