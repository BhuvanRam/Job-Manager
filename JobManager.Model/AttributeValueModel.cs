using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobManager.Model
{
    public class AttributeValueModel
    {

        public AttributeValueModel() { }

        public AttributeValueModel(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int AttributeId { get; set; }
        public int ParentValue { get; set; }
    }
}
