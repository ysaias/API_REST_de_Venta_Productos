
using AutoMapper;
using begywebsapi.DTOs;
using begywebsapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace begywebsapi.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RoleController: ControllerBase
    {
        private readonly VentasbegyContext _context;
        private readonly IMapper _mapper;

        public RoleController(VentasbegyContext context, IMapper mapper)
        {

            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<RoleDto>>> Get()
        {
            try
            {
                var listaRole = await _context.Roles.OrderBy(x => x.RoleNombre).ToListAsync();


                return _mapper.Map<List<RoleDto>>(listaRole);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreacionRoleDto creacionRoleDto)
        {

            try
            {
                var role = _mapper.Map<Role>(creacionRoleDto);

                _context.Add(role);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<RoleDto>> Get(int id)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == id);

            if (role == null)
            {
                return NotFound();
            }

            return _mapper.Map<RoleDto>(role);
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] CreacionRoleDto creacionRoleDto)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == id);

            if (role == null)
            {
                return NotFound();
            }

            role = _mapper.Map(creacionRoleDto, role);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == id);
            if (role == null)
            {
                return NotFound();
            }

            _context.Remove(role);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("buscarPorNombre/{nombre}")]
        public async Task<ActionResult<List<RoleDto>>> BuscarPorNombre(string nombre = "")
        {
            if (string.IsNullOrWhiteSpace(nombre)) { return new List<RoleDto>(); }

            return await _context.Roles
                .Where(x => x.RoleNombre.Contains(nombre))
                .OrderBy(x => x.RoleNombre)
                .Select(x => new RoleDto { Id = x.Id, RoleNombre = x.RoleNombre })
                .Take(5)
                .ToListAsync();
        }
    }
}