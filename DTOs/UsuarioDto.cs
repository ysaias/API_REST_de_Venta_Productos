using System.Data;

namespace begywebsapi.DTOs
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public RoleDto Role { get; set; }
        public string? CreatedAt { get; set; }
    }
}
