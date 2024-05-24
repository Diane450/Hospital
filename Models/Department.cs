using System;
using System.Collections.Generic;

namespace Hospital.Models;

public partial class Department
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<JobTitle> JobTitles { get; set; } = new List<JobTitle>();
}
