using System;
using System.Collections.Generic;

namespace begywebsapi.Models;

public partial class Producto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public int Stock { get; set; }

    public int? CategoriaId { get; set; }

    public string? ImagenUrl { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>(); // Relación con CartItems

    public virtual Categoria? Categoria { get; set; }

    public virtual ICollection<PedidoItem> PedidoItems { get; set; } = new List<PedidoItem>();
}
