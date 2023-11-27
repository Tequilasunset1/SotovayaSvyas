using System;
using System.Collections.Generic;

namespace SotovayaSvyas.Models;

public partial class TypeTariff
{
    public int TypeTariffId { get; set; }

    public string TariffName { get; set; } = null!;

    public virtual ICollection<TariffPlan> TariffPlans { get; set; } = new List<TariffPlan>();
}
