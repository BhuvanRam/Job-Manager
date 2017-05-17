using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobManager.Model
{
    public class Customer
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public int OrderAmount { get; set; }
    }
}
