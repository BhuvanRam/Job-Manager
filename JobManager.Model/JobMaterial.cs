﻿using System;
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

        public int Quantity { get; set; }

        public int? POId { get; set; }
        public string OrderedBy { get; set; }
        public DateTime? OrderedOn { get; set; }

        public bool IsSelected { get; set; }

    }
}
