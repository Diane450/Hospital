using System;
using System.Collections.Generic;

namespace Hospital.Models;

public partial class JobTitle
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int DepartmentId { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<Worker> Workers { get; set; } = new List<Worker>();
}
