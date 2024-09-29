namespace begywebsapi.DTOs
{
    public class PedidoItemDto
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; } = null!;
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
    }
}
