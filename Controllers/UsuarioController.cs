using AutoMapper;
using begywebsapi.DTOs;
using begywebsapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace begywebsapi.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly VentasbegyContext _context;
        private readonly IMapper _mapper;

        public UsuarioController(VentasbegyContext context, IMapper mapper)
        {

            _context = context;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<List<UsuarioDto>>> Get()
        {
            try
            {
                var listaRole = await _context.Usaurios
                    .Include(x => x.Role)
                    .OrderBy(x => x.Nombre).ToListAsync();


                return _mapper.Map<List<UsuarioDto>>(listaRole);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreacionUsuarioDto creacionUsuarioDto)
        {

            try
            {
                var usuario = _mapper.Map<Usaurio>(creacionUsuarioDto);
                usuario.CreatedAt = DateTime.Now;
                _context.Add(usuario);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<UsuarioDto>> Get(int id)
        {
            var usuario = await _context.Usaurios.Include(x => x.Role).FirstOrDefaultAsync(x => x.Id == id);

            if (usuario == null)
            {
                return NotFound();
            }

            return _mapper.Map<UsuarioDto>(usuario);
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] CreacionUsuarioDto creacionUsuarioDto)
        {
            var usuario = await _context.Usaurios.FirstOrDefaultAsync(x => x.Id == id);

            if (usuario == null)
            {
                return NotFound();
            }

            usuario = _mapper.Map(creacionUsuarioDto, usuario);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var usuario = await _context.Usaurios.FirstOrDefaultAsync(x => x.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Remove(usuario);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("buscarPorNombre/{nombre}")]
        public async Task<ActionResult<List<UsuarioDto>>> BuscarPorNombre(string nombre = "")
        {
            if (string.IsNullOrWhiteSpace(nombre)) { return new List<UsuarioDto>(); }

            return await _context.Usaurios
                .Where(x => x.Nombre.Contains(nombre))
                .OrderBy(x => x.Nombre)
                .Select(x => new UsuarioDto { Id = x.Id, Nombre = x.Nombre })
                .Take(5)
                .ToListAsync();
        }
    }
}