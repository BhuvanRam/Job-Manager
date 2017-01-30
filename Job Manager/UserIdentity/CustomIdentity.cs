using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Job_Manager.UserIdentity
{    
    public class CustomIdentity : IIdentity
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public int RoleId { get; private set; }
        //public int Roles { get; private set; }

        public string AuthenticationType
        {
            get
            {
                return "Custom Authentication";
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return !string.IsNullOrEmpty(Name);

            }
        }

        public CustomIdentity(string name, string email, int roleId)
        {
            Name = name;
            Email = email;
            RoleId = roleId;
        }


    }
}
