using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobManager.Model
{
    public class JobPOModel
    {
        public JobPOModel() { }
        public int JobId { get; set; }
        public int POId { get; set; }
        public string PONumber { get; set; }
        public int VendorId { get; set; }
        public int CreatedById { get; set; }

        public string CreatedBy { get; set; }
        public int ModifiedById { get; set; }
        public int? ApprovedById { get; set; }

        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }

        public int StatusId { get; set; }

        public decimal? Discount { get; set; }
        public decimal? Delivery { get; set; }
        public decimal? Payment { get; set; }
        public decimal? Packing { get; set; }
        public decimal? ExciseDuty { get; set; }
        public decimal? TaxesExtra { get; set; }
        public decimal? TransportInsurance { get; set; }
        public decimal? Transportation { get; set; }
        public decimal? Octroi { get; set; }

        public List<JobPOMaterialModel>  Materials { get; set; }
    }
    public class JobPOStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
