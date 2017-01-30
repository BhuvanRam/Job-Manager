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
    /// Interaction logic for UserMainScreen.xaml
    /// </summary>
    public partial class UserMainScreen : Window
    {
        public UserMainScreen()
        {
            InitializeComponent();
            LoadUsers();
        }
        DataAccess objDbDataAccess = new DataAccess();
        private void LoadUsers()
        {
            List<USER> lUsers =  objDbDataAccess.GetAllUsers();
            dUserGrid.ItemsSource = lUsers;
        }

        private void bAddUser_Click(object sender, RoutedEventArgs e)
        {
            UserCustomizationScreen objUserCustomizationScreen = new UserCustomizationScreen();
            objUserCustomizationScreen.ShowDialog();
        }

        private void EventSetter_OnHandler(object sender, MouseButtonEventArgs e)
        {
            object source = e.OriginalSource;
            string isAction = ((DataGridCell)sender).Column.Header.ToString().ToUpperInvariant();
            DataGridRow dgrSelectedRow = FindParent<DataGridRow>(sender as DependencyObject);
            USER selectedUser = (USER)dgrSelectedRow.DataContext;

            if (source.GetType() == typeof(Image) && isAction == "DELETE")
            {
                DataAccess objDataAccess = new DataAccess();
                bool isUserDeleted = objDataAccess.DeleteUser(selectedUser.UserID);
                if (isUserDeleted)
                MessageBox.Show("User Deleted Successfully");
                else
                    MessageBox.Show("User Deletion Failed!!!");                
            }


            if (source.GetType() == typeof(Image) && isAction == "EDIT")
            {
                UserCustomizationScreen objUserCustomizationScreen = new UserCustomizationScreen();
                objUserCustomizationScreen.EditUser = selectedUser;
                objUserCustomizationScreen.DataChanged += ObjUserScreen_DataChanged;
                objUserCustomizationScreen.ShowDialog();
            }
        }

        private void ObjUserScreen_DataChanged(object sender, EventArgs e)
        {
            LoadUsers();
        }

        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;
            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindParent<T>(parentObject);
        }
    }
}
