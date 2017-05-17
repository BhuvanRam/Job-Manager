using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobManager.Model
{
    public class JobPOMaterialModel
    {

        public JobPOMaterialModel() { }

        public int Id { get; set; }
        public string Name { get; set; }

        public string Attributes { get; set; }
        public string Type { get; set; }

        public int RequiredQuantity { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
