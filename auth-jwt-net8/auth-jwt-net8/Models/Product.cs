using System;
using System.Collections.Generic;

namespace auth_jwt_net8.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? Name { get; set; }

    public int? BrandId { get; set; }

    public decimal? Price { get; set; }

    public virtual Brand? Brand { get; set; }
}
