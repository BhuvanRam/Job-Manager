using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobManager.Model
{
    public class JobMaterialModel
    {
        public JobMaterialModel() { }

        public int Id { get; set; }
        public string Name { get; set; }

        public string Attributes { get; set; }
        public string Type { get; set; }

    }
}
