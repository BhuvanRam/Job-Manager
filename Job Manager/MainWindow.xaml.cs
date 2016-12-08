using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [PrincipalPermission(SecurityAction.Demand, Role = "Administrators")]
    public partial class MainWindow : Window,IView
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public IViewModel ViewModel
        {
            get
            {
                return DataContext as IViewModel;
            }
            set
            {
                DataContext = value;
            }
        }

        private void MenuSearchItem_Click(object sender, RoutedEventArgs e)
        {
            JobDetails JobWindow = new Job_Manager.JobDetails();
            JobWindow.ShowInTaskbar = false;
            JobWindow.Owner = this;
            JobWindow.ShowDialog();
        }

        private void MenuExitItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuAddAttribute_Click(object sender, RoutedEventArgs e)
        {
            Attribute_New objAttribute_New = new Attribute_New();
            objAttribute_New.ShowInTaskbar = false;
            objAttribute_New.Owner = this;
            objAttribute_New.ShowDialog();
        }
        private void MenuSearchAttribute_Click(object sender, RoutedEventArgs e)
        {
            AttributeMainScreen objAttribute = new AttributeMainScreen();
            objAttribute.ShowInTaskbar = false;
            objAttribute.Owner = this;
            objAttribute.ShowDialog();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MaterialMainScreen objMaterialMainScreen = new MaterialMainScreen();
            objMaterialMainScreen.ShowInTaskbar = false;
            objMaterialMainScreen.Owner = this;
            objMaterialMainScreen.ShowDialog();
        }

        private void MenuItemAddVendor_Click(object sender, RoutedEventArgs e)
        {
            VendorScreen objVendorScreen = new VendorScreen();
            objVendorScreen.ShowInTaskbar = false;
            objVendorScreen.Owner = this;
            objVendorScreen.ShowDialog();
        }
    }
}
