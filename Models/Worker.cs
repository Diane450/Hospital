using System;
using System.Collections.Generic;

namespace Hospital.Models;

public partial class Worker
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public int JobTitleId { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<DispensingDrug> DispensingDrugs { get; set; } = new List<DispensingDrug>();

    public virtual JobTitle JobTitle { get; set; } = null!;

    public virtual ICollection<ReceivingDrug> ReceivingDrugs { get; set; } = new List<ReceivingDrug>();

    public virtual User User { get; set; } = null!;
}
