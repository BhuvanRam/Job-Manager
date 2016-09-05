using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobManager.Model
{
    public class JobModel
    {
        public JobModel() { }

        public int JobId { get; set; }

        public string JobName { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<JobMaterialModel> Materials { get; set; }
    }
}
