using System;
using System.Collections.Generic;

namespace BurgerApi.Models;

public partial class Establishment
{
    public int EstablishmentId { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public int? Ownerid { get; set; }

    public virtual ICollection<EstablishmentProduct> EstablishmentProducts { get; set; } = new List<EstablishmentProduct>();

    public virtual ICollection<OpeningTime> OpeningTimes { get; set; } = new List<OpeningTime>();

    public virtual User? Owner { get; set; }
}
