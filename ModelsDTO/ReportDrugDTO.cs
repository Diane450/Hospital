using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.ModelsDTO
{
    public class ReportDrugDTO
    {
        public string Name { get; set; } = null!;

        public int DispensingCount { get; set; }

        public int ReceivingCount { get; set; }
    }
}
