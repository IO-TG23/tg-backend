namespace TG.Backend.Models.Auth
{
    public class AppUserChangePasswordDTO : IAppUserDTO
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string Token { get; set; }
    }
}
