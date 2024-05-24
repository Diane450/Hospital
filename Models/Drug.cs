using System;
using System.Collections.Generic;

namespace Hospital.Models;

public partial class Drug
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int ManufacturerId { get; set; }

    public int DrugProviderId { get; set; }

    public int Count { get; set; }

    public int TypeId { get; set; }

    public byte[] Photo { get; set; } = null!;

    public virtual ICollection<DispensingDrug> DispensingDrugs { get; set; } = new List<DispensingDrug>();

    public virtual DrugProvider DrugProvider { get; set; } = null!;

    public virtual Manufacturer Manufacturer { get; set; } = null!;

    public virtual ICollection<ReceivingDrug> ReceivingDrugs { get; set; } = new List<ReceivingDrug>();

    public virtual DrugType Type { get; set; } = null!;
}
