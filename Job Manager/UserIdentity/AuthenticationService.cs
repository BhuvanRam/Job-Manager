using JobManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Manager.UserIdentity
{
    public class AuthenticationService : IAuthenticationService
    {

        private class UserDataInDatabase
        {
            public UserDataInDatabase(string username, string email, string hashedPassword, string[] roles)
            {
                Username = username;
                Email = email;
                Password = hashedPassword;
                Roles = roles;
            }
            public string Username
            {
                get;
                private set;
            }

            public string Email
            {
                get;
                private set;
            }

            public string Password
            {
                get;
                private set;
            }

            public string[] Roles
            {
                get;
                private set;
            }
        }

        private static readonly List<UserDataInDatabase> _users = new List<UserDataInDatabase>()
        {
            new UserDataInDatabase("admin", "admin@JobManager.com","1234", new string[] { "Administrators" }),
            new UserDataInDatabase("user", "user@JobManager.com","1234", new string[] { })
        };


        public User AuthenticateUser(string username, string clearTextPassword)
        {
            UserDataInDatabase userData = _users.FirstOrDefault(u => u.Username.Equals(username)
                && u.Password.Equals(clearTextPassword));
            if (userData == null)
                throw new UnauthorizedAccessException("Access denied. Please provide some valid credentials.");

            return new User(userData.Username, userData.Email, userData.Roles);
        }    
    }
}
