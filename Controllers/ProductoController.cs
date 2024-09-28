
using begywebsapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using begywebsapi.DTOs;
using begywebsapi.Utilidades;

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

        [HttpGet]
        public async Task<ActionResult<List<ProductoDto>>> Get() {
            try
            {
                var listaProductos = await _context.Productos
                    .Include(x => x.Categoria)
                    .OrderBy(x => x.Nombre).ToListAsync();


                return _mapper.Map<List<ProductoDto>>(listaProductos);

            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }

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
            var producto = await _context.Productos.FirstOrDefaultAsync(x => x.Id == id);

            if (producto == null)
            {
                return NotFound();
            }

            producto = _mapper.Map(creacionProductoDto, producto);

            if (creacionProductoDto.ImagenUrl != null)
            {
                producto.ImagenUrl = await almacenadorArchivos.EditarArchico(contenedor, creacionProductoDto.ImagenUrl, producto.ImagenUrl);

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
    }
}
