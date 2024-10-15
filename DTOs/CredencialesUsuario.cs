using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace begywebsapi.DTOs
{
    public class CredencialesUsuario : IdentityUser
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
