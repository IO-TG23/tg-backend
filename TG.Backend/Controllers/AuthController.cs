using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TG.Backend.Features.Misc.Command;
using TG.Backend.Filters;
using TG.Backend.Models;

namespace TG.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IConfiguration _configuration;

        public AuthController(ISender sender, IConfiguration configuration)
        {
            _sender = sender;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AppUserRegisterDTO appUser)
        {
            AuthResponseModel resp = await _sender.Send(new RegisterUserCommand(appUser));

            return resp switch
            {
                { IsSuccess: false, StatusCode: HttpStatusCode.Unauthorized } => Unauthorized(new { resp.Messages }),
                { IsSuccess: true, StatusCode: HttpStatusCode.OK } => Ok(new
                { Message = resp.Messages.FirstOrDefault() }),
                _ => BadRequest()
            };
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AppUserLoginDTO appUser)
        {
            AuthResponseModel resp = await _sender.Send(new LoginUserCommand(appUser));

            return resp switch
            {
                { IsSuccess: false, StatusCode: HttpStatusCode.Unauthorized } => Unauthorized(new { resp.Messages }),
                { IsSuccess: true, StatusCode: HttpStatusCode.OK } => Ok(new
                { Message = resp.Messages.FirstOrDefault() }),
                _ => BadRequest()
            };
        }

        [HttpDelete("delete")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> Delete([FromBody] AppUserDeleteDTO appUser)
        {
            AuthResponseModel resp = await _sender.Send(new DeleteUserCommand(appUser));

            return resp switch
            {
                { IsSuccess: false, StatusCode: HttpStatusCode.Unauthorized } => Unauthorized(new { resp.Messages }),
                { IsSuccess: true, StatusCode: HttpStatusCode.NoContent } => NoContent(),
                _ => BadRequest()
            };
        }

        [HttpDelete("deleteOwn")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ServiceFilter(typeof(GetCurrentUserFromTheHeaderFilter))]
        public async Task<IActionResult> DeleteOwn()
        {
            AppUserDeleteDTO appUser = new() { Email = HttpContext.Items["email"].ToString() };

            AuthResponseModel resp = await _sender.Send(new DeleteUserCommand(appUser));

            HttpContext.Items["email"] = null;

            return resp switch
            {
                { IsSuccess: false, StatusCode: HttpStatusCode.Unauthorized } => Unauthorized(new { resp.Messages }),
                { IsSuccess: true, StatusCode: HttpStatusCode.NoContent } => NoContent(),
                _ => BadRequest()
            };
        }

        [HttpGet("resetPassword")]
        [ServiceFilter(typeof(ValidateAccountNotLockedFilter))]
        public async Task<IActionResult> ResetPassword([FromQuery] AppUserResetPasswordDTO appUser)
        {
            AuthResponseModel resp = await _sender.Send(new ResetPasswordCommand(appUser));

            return resp switch
            {
                { IsSuccess: false, StatusCode: HttpStatusCode.Unauthorized } => Unauthorized(new { resp.Messages }),
                { IsSuccess: true, StatusCode: HttpStatusCode.NoContent } => NoContent(),
                _ => BadRequest()
            };
        }

        [HttpPost("changePassword")]
        [ServiceFilter(typeof(ValidateAccountNotLockedFilter))]
        public async Task<IActionResult> ChangePassword([FromBody] AppUserChangePasswordDTO appUser)
        {
            AuthResponseModel resp = await _sender.Send(new ChangePasswordCommand(appUser));

            return resp switch
            {
                { IsSuccess: false, StatusCode: HttpStatusCode.Unauthorized } => Unauthorized(new { resp.Messages }),
                { IsSuccess: true, StatusCode: HttpStatusCode.NoContent } => NoContent(),
                _ => BadRequest()
            };
        }

        [HttpGet("confirmAccount")]
        public async Task<IActionResult> ConfirmAccount([FromQuery] AppUserConfirmAccountDTO appUser)
        {
            AuthResponseModel resp = await _sender.Send(new ConfirmAccountCommand(appUser.Token, appUser.Email));

            return resp switch
            {
                { IsSuccess: false, StatusCode: HttpStatusCode.Unauthorized } => Unauthorized(new { resp.Messages }),
                { IsSuccess: true, StatusCode: HttpStatusCode.NoContent } => Redirect(_configuration["ApplicationFrontend"]!),
                _ => BadRequest()
            };
        }

        [HttpPost("assignUserToRole")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> AssignUserToRole([FromBody] AssignUserToRoleDTO assignmentDTO)
        {
            AuthResponseModel resp = await _sender.Send(new AssignUserToRoleCommand(assignmentDTO));

            return resp switch
            {
                { IsSuccess: false, StatusCode: HttpStatusCode.NotFound } => NotFound(new { resp.Messages }),
                { IsSuccess: false, StatusCode: HttpStatusCode.BadRequest } => BadRequest(new { resp.Messages }),
                { IsSuccess: true, StatusCode: HttpStatusCode.OK } => Ok(new { resp.Messages }),
                _ => BadRequest()
            };
        }

        [HttpDelete("removeUserFromRole")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> RemoveUserFromRole([FromBody] RemoveUserFromRoleDTO removalDTO)
        {
            AuthResponseModel resp = await _sender.Send(new RemoveUserFromRoleCommand(removalDTO));

            return resp switch
            {
                { IsSuccess: false, StatusCode: HttpStatusCode.NotFound } => NotFound(new { resp.Messages }),
                { IsSuccess: true, StatusCode: HttpStatusCode.NoContent } => NoContent(),
                _ => BadRequest()
            };
        }

        [HttpPost("verifyJwtToken")]
        public async Task<IActionResult> VerifyJwtToken([FromBody] JwtTokenVerification Token)
        {
            bool res = await _sender.Send(new VerifyJWTQuery(Token));

            if (res)
            {
                return Ok();
            }

            return Unauthorized();
        }
    }
}