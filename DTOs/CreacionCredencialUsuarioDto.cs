using System.ComponentModel.DataAnnotations;

namespace begywebsapi.DTOs
{
    public class CreacionCredencialUsuarioDto
    {
        [Required]
        public string UserName { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        public string Telefono { get; set; }
    }
}
