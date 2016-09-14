using JobManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Manager.UserIdentity
{
    public interface IAuthenticationService
    {
        User AuthenticateUser(string username, string password);
    }
}
