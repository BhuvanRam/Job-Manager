﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Manager.UserIdentity
{
    public class AnonymousIdentity : CustomIdentity
    {
        public AnonymousIdentity() : base(string.Empty, string.Empty, 0)
        {

        }
    }
}
