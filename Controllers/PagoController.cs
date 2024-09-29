using AutoMapper;
using begywebsapi.DTOs;
using begywebsapi.Models;
using Microsoft.AspNetCore.Mvc;

namespace begywebsapi.Controllers
{
    [ApiController]
    [Route("api/pagos")]
    public class PagoController : ControllerBase
    {
        private readonly VentasbegyContext _context;
        private readonly IMapper _mapper;

        public PagoController(VentasbegyContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("{pedidoId:int}")]
        public async Task<ActionResult> CreatePago(int pedidoId, [FromBody] CreacionPagoDto creacionPagoDto)
        {
            // Buscar el pedido por su ID
            var pedido = await _context.Pedidos.FindAsync(pedidoId);

            if (pedido == null)
            {
                return NotFound("El pedido no existe.");
            }

            // Mapear el DTO a la entidad Pago
            var pago = _mapper.Map<Pago>(creacionPagoDto);
            pago.PedidoId = pedidoId; // Asegurarse de que el PedidoId esté establecido
            pago.FechaDePago = DateTime.Now; // Se puede establecer a DateTime.Now o desde el DTO si es necesario

            // Actualizar el estado del pedido
            pedido.Estado = "Pagado"; // Cambiar a "Pagado" o el estado que desees

            // Agregar el pago al contexto
            _context.Pagos.Add(pago);

            // Actualizar el pedido
            _context.Pedidos.Update(pedido);

            // Guardar todos los cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok("Pago registrado y estado del pedido actualizado exitosamente.");
            //return NoContent();
        }

    }

}
