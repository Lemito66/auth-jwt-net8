using System;
using System.Collections.Generic;

namespace auth_jwt_net8.Models;

public partial class Brand
{
    public int BrandId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
