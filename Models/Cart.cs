using System;
using System.Collections.Generic;

namespace begywebsapi.Models;

public partial class Cart
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Usuario Usuario { get; set; } = null!;
}
