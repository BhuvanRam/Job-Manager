using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobManager.Model
{
    public class User
    {

        public User(string username, string email, int roleId)
        {
            Username = username;
            EmailId = email;
            RoleId= roleId;
        }
        public User()
        {

        }

        public int UserId { get; set; }

        public string Username
        {
            get;
            set;
        }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public string EmailId
        {
            get;
            set;
        }

        public int RoleId
        {
            get;
            set;
        }

        public DateTime CreatedDate{ get; set; }
    }
}
