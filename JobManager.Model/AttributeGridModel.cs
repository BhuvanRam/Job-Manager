using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobManager.Model
{
    public class AttributeGridModel
    {
        public int AttributeId { get; set; }
        public string AttributeName { get; set; }
        public string AttributeParentName { get; set; }
        public int AttributeValueId { get; set; }
        public string AttributeValueName { get; set; }
        public string AttributeValueParentName { get; set; }
    }
}
