namespace TG.Backend.Models.Auth
{
    public class AssignUserToRoleDTO : IAppUserDTO
    {
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
