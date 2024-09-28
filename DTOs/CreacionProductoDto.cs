using begywebsapi.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace begywebsapi.DTOs
{
    public class CreacionProductoDto
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 50)]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int? CategoriaId { get; set; }
        public IFormFile? ImagenUrl { get; set; }
    }
}
