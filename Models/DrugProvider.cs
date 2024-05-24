using System;
using System.Collections.Generic;

namespace Hospital.Models;

public partial class DrugProvider
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public virtual ICollection<Drug> Drugs { get; set; } = new List<Drug>();
}
