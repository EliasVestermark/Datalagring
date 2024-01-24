using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class Ingridient
{
    public int IngridientId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
