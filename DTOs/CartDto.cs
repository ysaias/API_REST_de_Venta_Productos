namespace begywebsapi.DTOs
{
    public class CartDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();
    }
}
