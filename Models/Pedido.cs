using System;
using System.Collections.Generic;

namespace begywebsapi.Models;

public partial class Pedido
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public decimal Total { get; set; }

    public string Estado { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    public virtual ICollection<PedidoItem> PedidoItems { get; set; } = new List<PedidoItem>();

    public virtual Usuario Usuario { get; set; } = null!;
}
