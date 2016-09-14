using Job_Manager.UserIdentity;
using JobManager.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Job_Manager.ViewModel
{
    public class AuthenticationViewModel : INotifyPropertyChanged, IViewModel
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly DelegateCommand _loginCommand;
        private readonly DelegateCommand _logoutCommand;
        public event EventHandler OnRequestClose;

        private string _username;
        private string _status;

        public AuthenticationViewModel(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _loginCommand = new DelegateCommand(Login, CanLogin);
            _logoutCommand = new DelegateCommand(Logout, CanLogout);    
            
        }

        #region Properties
        public string Username
        {
            get { return _username; }
            set { _username = value; NotifyPropertyChanged("Username"); }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; NotifyPropertyChanged("Status"); }
        }


        public string AuthenticatedUser
        {
            get
            {
                if (IsAuthenticated)
                    return string.Format("Signed in as {0}. {1}",
                          Thread.CurrentPrincipal.Identity.Name,
                          Thread.CurrentPrincipal.IsInRole("Administrators") ? "You are an administrator!"
                              : "You are NOT a member of the administrators group.");

                return "Not authenticated!";
            }
        }


        #endregion

        #region Commands
        public DelegateCommand LoginCommand { get { return _loginCommand; } }

        public DelegateCommand LogoutCommand { get { return _logoutCommand; } }
        public DelegateCommand CloseWindowCommand { get; private set; }
        #endregion

        private void Login(object parameter)
        {
            
            PasswordBox passwordBox = parameter as PasswordBox;
            string clearTextPassword = passwordBox.Password;
            try
            {
                //Validate credentials through the authentication service
                User user = _authenticationService.AuthenticateUser(Username, clearTextPassword);

                //Get the current principal object
                CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
                if (customPrincipal == null)
                    throw new ArgumentException("The application's default thread principal must be set to a CustomPrincipal object on startup.");

                //Authenticate the user
                customPrincipal.Identity = new CustomIdentity(user.Username, user.Email, user.Roles);
                MainWindow objMainWindow = new MainWindow();
                objMainWindow.Show();
                OnRequestClose(this, new EventArgs());              
            }
            catch (UnauthorizedAccessException)
            {
                Status = "Login failed! Please provide some valid credentials.";
            }
            catch (Exception ex)
            {
                Status = string.Format("ERROR: {0}", ex.Message);
            }
        }

        private bool CanLogin(object parameter)
        {
            return !IsAuthenticated;
        }

        private void Logout(object parameter)
        {
            CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
            if (customPrincipal != null)
            {
                customPrincipal.Identity = new AnonymousIdentity();
                NotifyPropertyChanged("AuthenticatedUser");
                NotifyPropertyChanged("IsAuthenticated");
                _loginCommand.RaiseCanExecuteChanged();
                _logoutCommand.RaiseCanExecuteChanged();
                Status = string.Empty;
            }
        }

        private bool CanLogout(object parameter)
        {
            return IsAuthenticated;
        }

        public bool IsAuthenticated
        {
            get { return Thread.CurrentPrincipal.Identity.IsAuthenticated; }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
