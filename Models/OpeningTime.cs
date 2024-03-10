using System;
using System.Collections.Generic;

namespace BurgerApi.Models;

public partial class OpeningTime
{
    public int OpeningTimeId { get; set; }

    public int? EstablishmentId { get; set; }

    public string? DayOfWeek { get; set; }

    public TimeOnly? OpenTime { get; set; }

    public TimeOnly? CloseTime { get; set; }

    public virtual Establishment? Establishment { get; set; }
}
