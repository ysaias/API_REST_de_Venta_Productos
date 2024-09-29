namespace begywebsapi.DTOs
{
    public class CreacionPagoDto
    {
        public int PedidoId { get; set; }
        public DateTime FechaDePago { get; set; } = DateTime.Now;
        public string MetodoDePago { get; set; } = null!;
        public decimal MontoDePago { get; set; }
    }
}
