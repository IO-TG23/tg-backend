namespace TG.Backend.Models.Auth
{
    public class RemoveUserFromRoleDTO : IAppUserDTO
    {
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
