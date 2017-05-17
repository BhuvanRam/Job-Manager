using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobManager.Model
{
    public class ReportPOContent
    {
        public int Sno { get; set; }
        public string Particulars { get; set; }
        public int Quantity { get; set; }
        public int Units { get; set; }
        public int UnitRate { get; set; }
        public int Amount { get; set; }
    }
}
