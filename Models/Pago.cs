using System;
using System.Collections.Generic;

namespace begywebsapi.Models;

public partial class Pago
{
    public int Id { get; set; }

    public int PedidoId { get; set; }

    public DateTime FechaDePago { get; set; }

    public string MetodoDePago { get; set; } = null!;

    public decimal MontoDePago { get; set; }

    public virtual Pedido Pedido { get; set; } = null!;
}
