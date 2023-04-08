namespace TG.Backend.Models.Auth
{
    public class AppUserRegisterDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
