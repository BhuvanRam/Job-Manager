using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobManager.Model
{
    public class Company
    {
        public int companyId { get; set; }
        public string companyName { get; set; }
        public string companyAddress { get; set; }
        public string PhoneNo { get; set; }
        public string FaxNo { get; set; }
        public string Email { get; set; }
        public string TinNo { get; set; }
        public string CstNo { get; set; }
        public string EccNo { get; set; }
        public string RangeDivision { get; set; }
        public string BuyerName { get; set; }
        public string BuyerPhoneNo { get; set; }
    }
}
