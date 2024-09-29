using AutoMapper;
using begywebsapi.DTOs;
using begywebsapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace begywebsapi.Controllers
{
    [Route("api/pedido")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly VentasbegyContext _context;
        private readonly IMapper _mapper;

        public PedidoController(VentasbegyContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<PedidoDto>>> Get()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.PedidoItems)
                .Include(p => p.Pagos)
                .ToListAsync();

            return _mapper.Map<List<PedidoDto>>(pedidos);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreacionPedidoDto creacionPedidoDto)
        {
            var pedido = _mapper.Map<Pedido>(creacionPedidoDto);

            pedido.CreatedAt = DateTime.Now;
            _context.Add(pedido);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("usuario/{usuarioId:int}")]
        public async Task<ActionResult<List<PedidoDto>>> GetByUsuarioId(int usuarioId)
        {
            var pedidos = await _context.Pedidos
                .Where(p => p.UsuarioId == usuarioId)  // Filtrar por UsuarioId
                .Include(p => p.PedidoItems)
                .ThenInclude(pi => pi.Producto)  // Incluir los productos dentro de los items del pedido (si es necesario)
                .Include(p => p.Pagos)
                .ToListAsync();

            if (pedidos == null || !pedidos.Any())
            {
                return NotFound("No se encontraron pedidos para este usuario.");
            }

            // Mapear la lista de pedidos a PedidoDto
            return _mapper.Map<List<PedidoDto>>(pedidos);
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] CreacionPedidoDto creacionPedidoDto)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }

            _mapper.Map(creacionPedidoDto, pedido);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpPost("{usuarioId:int}/from-cart")]
        public async Task<ActionResult> CreatePedidoFromCart(int usuarioId)
        {
            // Obtener el carrito del usuario
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Producto)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

            if (cart == null || !cart.CartItems.Any())
            {
                return BadRequest("El carrito está vacío o no existe");
            }


            var pedido = new Pedido
            {
                UsuarioId = usuarioId,
                Estado = "Pendiente",  // Estado inicial del pedido. Esto se deb resolver de alguna manera, luego lo poensaré
                CreatedAt = DateTime.Now
            };

            decimal total = 0;

            // Crear los items del pedido y actualizar el stock
            foreach (var cartItem in cart.CartItems)
            {
                var producto = cartItem.Producto;

                // Verificar que haya stock disponible
                if (producto.Stock < cartItem.Cantidad)
                {
                    return BadRequest($"No hay suficiente stock para el producto {producto.Nombre}");
                }

                // Actualizar el stock del producto
                producto.Stock -= cartItem.Cantidad;



                // Crear el PedidoItem
                var pedidoItem = new PedidoItem
                {
                    ProductoId = producto.Id,
                    Cantidad = cartItem.Cantidad,
                    Precio = producto.Precio
                };

                pedido.PedidoItems.Add(pedidoItem);

                // Calcular el total del pedido
                total += cartItem.Cantidad * producto.Precio;
            }

            // Asignar el total calculado al pedido
            pedido.Total = total;

            // Guardar el pedido en la base de datos
            _context.Pedidos.Add(pedido);

            // Eliminar manualmente todos los CartItems antes de eliminar el Cart 
            _context.CartItems.RemoveRange(cart.CartItems);

            // Luego eliminar el carrito
            _context.Carts.Remove(cart);

            // Guardar todos los cambios
            await _context.SaveChangesAsync();

            return Ok("Pedido creado exitosamente y carrito vaciado");
        }


    }
}