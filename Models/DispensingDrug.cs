using System;
using System.Collections.Generic;

namespace Hospital.Models;

public partial class DispensingDrug
{
    public int Id { get; set; }

    public int DrugId { get; set; }

    public int WorkerId { get; set; }

    public DateOnly Date { get; set; }

    public virtual Drug Drug { get; set; } = null!;

    public virtual Worker Worker { get; set; } = null!;
}
