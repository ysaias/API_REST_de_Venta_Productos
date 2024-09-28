using AutoMapper;
using begywebsapi.DTOs;
using begywebsapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace begywebsapi.Controllers
{
    [Route("api/carts")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly VentasbegyContext _context;
        private readonly IMapper _mapper;

        public CartController(VentasbegyContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Obtener el carrito de un usuario
        [HttpGet("{usuarioId:int}")]
        public async Task<ActionResult<CartDto>> GetCart(int usuarioId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Producto)  // Incluyendo la información del producto
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

            if (cart == null)
            {
                return NotFound("Carrito no encontrado");
            }

            return _mapper.Map<CartDto>(cart);
        }

        // Agregar un producto al carrito
        [HttpPost("{usuarioId:int}/items")]
        public async Task<ActionResult> AddItemToCart(int usuarioId, [FromBody] CreacionCartItemDto creacionCartItemDto)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

            if (cart == null)
            {
                // Si el carrito no existe, se crea uno nuevo
                cart = new Cart
                {
                    UsuarioId = usuarioId,
                    CreatedAt = DateTime.Now
                };
                _context.Carts.Add(cart);
            }

            var producto = await _context.Productos.FindAsync(creacionCartItemDto.ProductoId);
            if (producto == null)
            {
                return NotFound("Producto no encontrado");
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductoId == creacionCartItemDto.ProductoId);
            if (cartItem != null)
            {
                // Si el producto ya está en el carrito, solo se actualiza la cantidad
                cartItem.Cantidad += creacionCartItemDto.Cantidad;
            }
            else
            {
                // Si no, se agrega un nuevo elemento al carrito
                cartItem = new CartItem
                {
                    ProductoId = creacionCartItemDto.ProductoId,
                    Cantidad = creacionCartItemDto.Cantidad,
                    CartId = cart.Id // Aquí el CartId se asocia correctamente
                };
                cart.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }


        // Eliminar un producto del carrito
        [HttpDelete("{usuarioId:int}/items/{productoId:int}")]
        public async Task<ActionResult> RemoveItemFromCart(int usuarioId, int productoId)
        {
            var cart = await _context.Carts
                 .Include(c => c.CartItems)
                 .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

            if (cart == null)
            {
                return NotFound("Carrito no encontrado");
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductoId == productoId);
            if (cartItem == null)
            {
                return NotFound("Producto no encontrado en el carrito");
            }

            _context.CartItems.Remove(cartItem); // Eliminar directamente
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Vaciar el carrito
        [HttpDelete("{usuarioId:int}/clear")]
        public async Task<ActionResult> ClearCart(int usuarioId)
        {
            var cart = await _context.Carts
         .Include(c => c.CartItems)
         .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

            if (cart == null)
            {
                return NotFound("Carrito no encontrado");
            }

            _context.CartItems.RemoveRange(cart.CartItems); // Eliminar directamente los CartItems
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}