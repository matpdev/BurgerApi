using System;
using System.Collections.Generic;

namespace BurgerApi.Models;

public partial class EstablishmentProduct
{
    public int EstablishmentProductId { get; set; }

    public int? EstablishmentId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public string? Category { get; set; }

    public bool? Availability { get; set; }

    public virtual Establishment? Establishment { get; set; }
}
