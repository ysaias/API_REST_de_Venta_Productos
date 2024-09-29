namespace begywebsapi.DTOs
{
    public class CreacionPedidoItemDto
    {
        public int PedidoId { get; set; }
        public PedidoDto Pedido { get; set; }
        public int ProductoId { get; set; }
        public ProductoDto Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
    }
}
