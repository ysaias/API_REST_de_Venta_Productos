using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace begywebsapi.DTOs
{
    public class productoPutGetDto
    {
        public ProductoDto Producto { get; set; } 
        
        public List<CategoriaDto> CategoriasNoSeleccionados { get; set; }
       
    }
}
