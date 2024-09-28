using begywebsapi.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace begywebsapi.DTOs
{
    public class CreacionRoleDto
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 50)]
        [PrimeraLetraMayuscula]
        public string RoleNombre { get; set; }
    }
}
