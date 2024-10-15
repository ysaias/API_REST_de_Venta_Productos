using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace begywebsapi.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Apellido { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public int? Telefono { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    public virtual Role Role { get; set; } = null!;
}
