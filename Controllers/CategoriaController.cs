using AutoMapper;
using begywebsapi.DTOs;
using begywebsapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace begywebsapi.Controllers
{
    [Route("api/categorias")]
    [ApiController]
    public class CategoriaController: ControllerBase
    {
        private readonly VentasbegyContext _context;
        private readonly IMapper _mapper;

        public CategoriaController(VentasbegyContext context, IMapper mapper)
        {

            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoriaDto>>> Get()
        {
            try
            {
                var listaCategoria = await _context.Categorias.OrderBy(x => x.Nombre).ToListAsync();


                return _mapper.Map<List<CategoriaDto>>(listaCategoria);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreacionCategoriaDto creacionCategoriaDto)
        {

            try
            {
                var categoria = _mapper.Map<Categoria>(creacionCategoriaDto);

                _context.Add(categoria);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoriaDto>> Get(int id)
        {
            var categoria = await _context.Categorias.FirstOrDefaultAsync(x => x.Id == id);

            if (categoria == null)
            {
                return NotFound();
            }

            return _mapper.Map<CategoriaDto>(categoria);
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] CreacionCategoriaDto creacionCategoriaDto)
        {
            var categoria = await _context.Categorias.FirstOrDefaultAsync(x => x.Id == id);

            if (categoria == null)
            {
                return NotFound();
            }

            categoria = _mapper.Map(creacionCategoriaDto, categoria);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var categoria = await _context.Categorias.FirstOrDefaultAsync(x => x.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }

            _context.Remove(categoria);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("buscarPorNombre/{nombre}")]
        public async Task<ActionResult<List<CategoriaDto>>> BuscarPorNombre(string nombre = "")
        {
            if (string.IsNullOrWhiteSpace(nombre)) { return new List<CategoriaDto>(); }

            return await _context.Categorias
                .Where(x => x.Nombre.Contains(nombre))
                .OrderBy(x => x.Nombre)
                .Select(x => new CategoriaDto { Id = x.Id, Nombre = x.Nombre})
                .Take(5)
                .ToListAsync();
        }
    }
}