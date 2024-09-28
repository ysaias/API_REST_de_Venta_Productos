namespace begywebsapi.DTOs
{
    public class PedidoDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public UsuarioDto Usuario { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<PedidoItemDto> PedidoItems { get; set; }
    }
}
