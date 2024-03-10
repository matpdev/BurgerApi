using System;
using System.Collections.Generic;

namespace BurgerApi.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int? UserId { get; set; }

    public decimal? TotalAmount { get; set; }

    public DateOnly? PaymentDate { get; set; }

    public string? PaymentMethod { get; set; }

    public virtual User? User { get; set; }
}
