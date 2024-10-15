
using begywebsapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using begywebsapi.DTOs;
using begywebsapi.Utilidades;
using Microsoft.AspNetCore.Authorization;

namespace begywebsapi.Controllers
{
    
    [Route("api/productos")]
    [ApiController]
    public class ProductoController: ControllerBase
    {
        private readonly VentasbegyContext _context;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly IMapper _mapper;
        private readonly string contenedor = "producto";

        public ProductoController(VentasbegyContext context, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos) { 
            
            _context = context;
            _mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }

       

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] CreacionProductoDto creacionProductoDto)
        {
            var producto = _mapper.Map<Producto>(creacionProductoDto);
            producto.CreatedAt = DateTime.Now;

            if (creacionProductoDto.ImagenUrl != null)
            {
                producto.ImagenUrl = await almacenadorArchivos.GuardarArchivo(contenedor, creacionProductoDto.ImagenUrl);
            }

           
            _context.Add(producto);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductoDto>> Get(int id)
        {
            var producto = await _context.Productos.FirstOrDefaultAsync(x => x.Id == id);

            if (producto == null)
            {
                return NotFound();
            }

            return _mapper.Map<ProductoDto>(producto);
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] CreacionProductoDto creacionProductoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Esto proporcionará detalles sobre qué campos son incorrectos
            }

            var producto = await _context.Productos.FirstOrDefaultAsync(x => x.Id == id);

            if (producto == null)
            {
                return NotFound();
            } 

            // Mapear los demás campos
            producto = _mapper.Map(creacionProductoDto, producto);
            

            // Verifica si se ha recibido una nueva imagen
            if (creacionProductoDto.ImagenUrl != null && creacionProductoDto.ImagenUrl.Length > 0)
            {
                // Si hay una nueva imagen, se actualiza
                producto.ImagenUrl = await almacenadorArchivos.EditarArchico(contenedor, creacionProductoDto.ImagenUrl, producto.ImagenUrl);
            }
            else
            {
                // Si no se selecciona una nueva imagen, conservar la imagen actual
                producto.ImagenUrl = producto.ImagenUrl;  // Mantener la imagen original
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var producto = await _context.Productos.FirstOrDefaultAsync(x => x.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            _context.Remove(producto);
            await _context.SaveChangesAsync();
            await almacenadorArchivos.BorrarArchivo(producto.ImagenUrl, contenedor);
            return NoContent();
        }

        [HttpGet("buscarPorNombre/{nombre}")]
        public async Task<ActionResult<List<ProductoDto>>> BuscarPorNombre(string nombre = "")
        {
            if (string.IsNullOrWhiteSpace(nombre)) { return new List<ProductoDto>(); }

            return await _context.Productos
                .Where(x => x.Nombre.Contains(nombre))
                .OrderBy(x => x.Nombre)
                .Select(x => new ProductoDto { Id = x.Id, Nombre = x.Nombre, ImagenUrl = x.ImagenUrl })
                .Take(5)
                .ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<LandingPageDto>> GetProductos()
        {
            var top = 6;

            var productos = await _context.Productos
                .OrderBy(x => x.CreatedAt)
                .Take(top)
                .ToListAsync();

            var resultado = new LandingPageDto();
            resultado.Productos = _mapper.Map<List<ProductoDto>>(productos);

            return resultado;
        }

        [HttpGet("postget")]
        public async Task<ActionResult<CategoriasPostGetDTO>> PostGet()
        {
            var categorias = await _context.Categorias.ToListAsync();
            

            var categoriasDTO = _mapper.Map<List<CategoriaDto>>(categorias);

            return new CategoriasPostGetDTO { Categorias = categoriasDTO };
        }

        [HttpGet("PutGet/{id:int}")]
        public async Task<ActionResult<productoPutGetDto>> PutGet(int id)
        {
            var productoActionResult = await Get(id);
            if (productoActionResult.Result is NotFoundResult) { return NotFound(); }

            var producto = productoActionResult.Value;

            // Obtener la lista de categorías no seleccionadas
            var categoriasNoSeleccionados = await _context.Categorias
                .Where(x => x.Id != producto.CategoriaId)  // Mostrar todas excepto la categoría ya seleccionada
                .ToListAsync();

            var categoriasNoSelecionadosDTO = _mapper.Map<List<CategoriaDto>>(categoriasNoSeleccionados);

            var respuesta = new productoPutGetDto
            {
                Producto = producto,
                CategoriasNoSeleccionados = categoriasNoSelecionadosDTO
            };

            return respuesta;

        }

        [HttpGet("filtrar")]
        public async Task<ActionResult<List<ProductoDto>>> Filtrar([FromQuery] ProductoFiltrarDto productoFiltrarDto)
        {
            var productosQueryable = _context.Productos.AsQueryable();

            if (!string.IsNullOrEmpty(productoFiltrarDto.Nombre))
            {
                productosQueryable = productosQueryable.Where(x => x.Nombre.Contains(productoFiltrarDto.Nombre));
            }

            if (productoFiltrarDto.FechaDesde.HasValue)
            {
                productosQueryable = productosQueryable.Where(x => x.CreatedAt >= productoFiltrarDto.FechaDesde.Value);
            }

            if (productoFiltrarDto.FechaHasta.HasValue)
            {
                productosQueryable = productosQueryable.Where(x => x.CreatedAt <= productoFiltrarDto.FechaHasta.Value); 
            }

            if(productoFiltrarDto.CategoriaId != 0)
            {
                productosQueryable = productosQueryable.Where(x => x.CategoriaId == productoFiltrarDto.CategoriaId);    
            }

            if (productoFiltrarDto.PrecioMin != 0)
            {
                productosQueryable = productosQueryable.Where(x => x.Precio >= productoFiltrarDto.PrecioMin);
            }

            if (productoFiltrarDto.PrecioMax != 0)
            {
                productosQueryable = productosQueryable.Where(x => x.Precio <= productoFiltrarDto.PrecioMax);
            }

            await HttpContext.InsertarParametrosPaginacionEnCabecera(productosQueryable);   

            var productos = await productosQueryable.Paginar(productoFiltrarDto.PaginacionDTO).ToListAsync();
            
            return _mapper.Map<List<ProductoDto>>(productos);
        }

    }
}
