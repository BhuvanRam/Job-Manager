using JobManager.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Job_Manager
{
    /// <summary>
    /// Interaction logic for UserCustomizationScreen.xaml
    /// </summary>
    public partial class UserCustomizationScreen : Window
    {
        DataAccess objdbDataAccess = new DataAccess();
        public delegate void DataChangedEventHandler(object sender, EventArgs e);
        public event DataChangedEventHandler DataChanged;
        private USER _EditUser;

        public USER EditUser
        {
            get
            {
                return _EditUser;
            }

            set
            {
                _EditUser = value;
                SetEditFields();
            }
        }

        private void SetEditFields()
        {
            txtUserName.IsEnabled = false;
            txtUserName.Text = EditUser.UserName;
            txtPassword.Password = EditUser.Password;
            txtConfirmPassword.Password = EditUser.ConfirmPassword;
            txtEmailId.Text = EditUser.EmailId;
            cRoles.SelectedValue = EditUser.RoleId;            
        }

        public UserCustomizationScreen()
        {
            InitializeComponent();
            FillRoles();
        }
        
        public void FillRoles()
        {
            List<ROLE> lRoles = objdbDataAccess.GetAllRoles();
            cRoles.ItemsSource = lRoles;
        }

        private void bUserNameAvailability_Click(object sender, RoutedEventArgs e)
        {
            if (objdbDataAccess.CheckUserNameAvailability(txtUserName.Text))
            {
                MessageBox.Show("Username already Exists!!!", "Username Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
                MessageBox.Show("Username is available!!!");
        }

        private void btnClearUser_Click(object sender, RoutedEventArgs e)
        {
            clearFields();
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            if (EditUser!=null && objdbDataAccess.CheckUserNameAvailability(txtUserName.Text))
            {
                MessageBox.Show("Username already Exists!!!", "Username Error", MessageBoxButton.OK, MessageBoxImage.Error);
                clearFields();
                return;
            }
            USER objnewUser = new USER();
            objnewUser.UserName = txtUserName.Text;
            objnewUser.Password = txtPassword.Password;
            objnewUser.ConfirmPassword = txtConfirmPassword.Password;
            objnewUser.EmailId = txtEmailId.Text;
            objnewUser.CreatedDate = DateTime.Now;
            ROLE role = (ROLE)cRoles.SelectedItem;
            objnewUser.RoleId = Convert.ToInt32(role.RoleID);
            bool isNewUserInserted = objdbDataAccess.saveUser(objnewUser);
            if (isNewUserInserted)
            {
                MessageBox.Show("User Inserted Successfully");
                clearFields();
            }
        }

        public void clearFields()
        {
            txtUserName.IsEnabled = true;
            txtUserName.Text = string.Empty;
            txtPassword.Password = string.Empty;
            txtConfirmPassword.Password = string.Empty;
            txtEmailId.Text = string.Empty;
            cRoles.SelectedItem = null;
        }
    }
}
