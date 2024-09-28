using System;
using System.Collections.Generic;

namespace begywebsapi.Models;

public partial class CartItem
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }

    public virtual Cart Cart { get; set; } = null!; // Propiedad de navegación al carrito
    public virtual Producto Producto { get; set; } = null!; // Propiedad de navegación al producto
}
