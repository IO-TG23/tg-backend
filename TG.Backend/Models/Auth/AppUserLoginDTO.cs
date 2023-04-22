namespace TG.Backend.Models.Auth
{
    public class AppUserLoginDTO : IAppUserDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Code { get; set; }
    }
}
