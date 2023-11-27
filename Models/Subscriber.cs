using System;
using System.Collections.Generic;

namespace SotovayaSvyas.Models;

public partial class Subscriber
{
    public int SubscriberId { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string PassportDetails { get; set; } = null!;

    public virtual ICollection<ServicesProvided> ServicesProvideds { get; set; } = new List<ServicesProvided>();

    public virtual ICollection<Treaty> Treaties { get; set; } = new List<Treaty>();
}
