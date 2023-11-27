using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SotovayaSvyas.Models;

public partial class ServicesProvided
{
    public int ServicesProvidedId { get; set; }
    [DataType(DataType.Time)]
    public TimeOnly Time { get; set; }

    public int QuantitySms { get; set; }

    public int DataVolume { get; set; }

    public int SubscriberId { get; set; }

    public virtual Subscriber? Subscriber { get; set; } = null!;
}
