using begywebsapi.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace begywebsapi.DTOs
{
    public class CreacionUsuarioDto
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 50)]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El campo {2} es requerido")]
        [StringLength(maximumLength: 255)]
        [ValidacionEmail]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo {3} es requerido")]
        [ValidacionPassword]
        public string Password { get; set; }

        [Required(ErrorMessage = "El campo {4} es requerido")]
        public int RoleId { get; set; }
    }
}
