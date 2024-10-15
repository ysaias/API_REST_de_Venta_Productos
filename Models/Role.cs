using System;
using System.Collections.Generic;

namespace begywebsapi.Models;

public partial class Role
{
    public int Id { get; set; }

    public string RoleNombre { get; set; } = null!;

    public virtual ICollection<Usuario> Usaurios { get; set; } = new List<Usuario>();
}
