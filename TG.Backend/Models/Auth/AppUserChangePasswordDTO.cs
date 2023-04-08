namespace TG.Backend.Models.Auth
{
    public class AppUserChangePasswordDTO
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string Token { get; set; }
    }
}
