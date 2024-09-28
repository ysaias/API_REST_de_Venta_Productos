namespace begywebsapi.DTOs
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; }  // Para devolver información adicional del producto
        public int Cantidad { get; set; }
    }
}
