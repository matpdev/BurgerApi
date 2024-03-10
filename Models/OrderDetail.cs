using System;
using System.Collections.Generic;

namespace BurgerApi.Models;

public partial class OrderDetail
{
    public int OrderId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Subtotal { get; set; }

    public virtual Product? Product { get; set; }
}
