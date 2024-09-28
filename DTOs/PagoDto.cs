namespace begywebsapi.DTOs
{
    public class PagoDto
    {
        public int Id { get; set; }
        public int PedidoId { get; set; } // Foreign Key
        public PedidoDto Pedido { get; set; } // Relación con Pedido
        public decimal MontoDePago { get; set; }
        public DateTime FechaDePago { get; set; }
        public string MetodoDePago { get; set; }
    }
}
