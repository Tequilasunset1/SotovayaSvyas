using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SotovayaSvyas.Models;

public partial class Treaty
{
    public int TreatyId { get; set; }

    public int SubscriberId { get; set; }

    public int TariffPlanId { get; set; }
    [DataType(DataType.Date)]
    public DateOnly DateConclusion { get; set; }
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public virtual Subscriber? Subscriber { get; set; } = null!;

    public virtual TariffPlan? TariffPlan { get; set; } = null!;
}
