using System;
using System.Collections.Generic;

namespace SotovayaSvyas.Models;

public partial class TariffPlan
{
    public int TariffPlanId { get; set; }

    public string TariffName { get; set; } = null!;

    public decimal SubscriptionLocal { get; set; }

    public decimal SubscriptionIntercity { get; set; }

    public decimal SubscriptionInternational { get; set; }

    public int TypeTariffId { get; set; }

    public int PriceSms { get; set; }

    public virtual ICollection<Treaty> Treaties { get; set; } = new List<Treaty>();

    public virtual TypeTariff? TypeTariff { get; set; } = null!;
}
