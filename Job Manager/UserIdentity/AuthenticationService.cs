using JobManager.DAL;
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
        DataAccess objDataAccess = new DataAccess();

        //private class UserDataInDatabase
        //{
           
        //    public UserDataInDatabase(string username, string email, string hashedPassword, string[] roles)
        //    {
        //        Username = username;
        //        Email = email;
        //        Password = hashedPassword;
        //        Roles = roles;
        //    }
        //    public string Username
        //    {
        //        get;
        //        private set;
        //    }

        //    public string Email
        //    {
        //        get;
        //        private set;
        //    }

        //    public string Password
        //    {
        //        get;
        //        private set;
        //    }

        //    public string[] Roles
        //    {
        //        get;
        //        private set;
        //    }
        //}

        //private static readonly List<UserDataInDatabase> _users = new List<UserDataInDatabase>()
        //{
        //    new UserDataInDatabase("admin", "admin@JobManager.com","1234", new string[] { "Administrators" }),
        //    new UserDataInDatabase("user", "user@JobManager.com","1234", new string[] { })
        //};


        public USER AuthenticateUser(string username, string clearTextPassword)
        {

           // UserDataInDatabase userData 
           USER AuthenticatedUser = objDataAccess.AuthenticateUser(username,clearTextPassword);
                //_users.FirstOrDefault(u => u.Username.Equals(username)
               // && u.Password.Equals(clearTextPassword));
            if (AuthenticatedUser.UserID == 0)
                throw new UnauthorizedAccessException("Access denied. Please provide some valid credentials.");

            return AuthenticatedUser;
        }    
    }
}
