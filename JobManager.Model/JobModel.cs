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

        public int StatusId { get; set; }

        public List<JobMaterialModel> Materials { get; set; }
    }

    public class JobStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
