namespace TG.Backend.Models.Auth
{
    public class AppUserConfirmAccountDTO : IAppUserDTO
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
