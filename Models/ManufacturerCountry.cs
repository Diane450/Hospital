using System;
using System.Collections.Generic;

namespace Hospital.Models;

public partial class ManufacturerCountry
{
    public int Id { get; set; }

    public string Country { get; set; } = null!;

    public virtual ICollection<Manufacturer> Manufacturers { get; set; } = new List<Manufacturer>();
}
