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
    /// Interaction logic for VendorMainScreen.xaml
    /// </summary>
    public partial class VendorMainScreen : Window
    {
        
        

        private List<Vendor> _lvendors = new List<Vendor>();

        public List<Vendor> ListOfVendors
        {
            get
            {
                return _lvendors;
            }

            set
            {
                _lvendors = value;
            }
        }


        public VendorMainScreen()
        {
            InitializeComponent();
            LoadVendorSearchDropdown();
            LoadVendors();
        }

        public void LoadVendorSearchDropdown()
        {
            cVendorFields.Items.Add("Vendor Name");
            cVendorFields.Items.Add("Vendor Id");
        }

        public void LoadVendors()
        {
            DataAccess objDataAccess = new DataAccess();
            ListOfVendors = objDataAccess.GetAllVendors();
            dVendorGrid.ItemsSource = null;
            dVendorGrid.ItemsSource = ListOfVendors;
        }

        private void VendorSearch_Click(object sender, RoutedEventArgs e)
        {
            DataAccess objDataAccess = new DataAccess();
            string sSearchCriteria = txtSearch.Text;
            string sVendorColumn = cVendorFields.Text;

            if (!string.IsNullOrEmpty(sSearchCriteria))
            {
                switch (sVendorColumn)
                {
                    case "Vendor Name":

                        ListOfVendors = objDataAccess.GetVendorByName(sSearchCriteria);
                        dVendorGrid.ItemsSource = null;
                        dVendorGrid.ItemsSource = ListOfVendors;

                        break;
                    case "Vendor Id":
                        Vendor objVendor = objDataAccess.GetVendor(sSearchCriteria);
                        if (objVendor != null)
                        {
                            ListOfVendors.Clear();
                            ListOfVendors.Add(objVendor);

                            dVendorGrid.ItemsSource = null;
                            dVendorGrid.ItemsSource = ListOfVendors;
                        }
                        else
                            MessageBox.Show("Not able to find Vendor Id");
                        break;
                    default:
                        break;
                }
            }
            else
                MessageBox.Show("Select Search Criteria!!!");       
        }

        private void VendorAdd_Click(object sender, RoutedEventArgs e)
        {
            VendorScreen objVendorScreen = new VendorScreen();
            objVendorScreen.DataChanged += ObjVendorScreen_DataChanged;
            objVendorScreen.ShowDialog();
            
        }

        private void EventSetter_OnHandler(object sender, MouseButtonEventArgs e)
        {
            object source = e.OriginalSource;
            string isAction = ((DataGridCell)sender).Column.Header.ToString().ToUpperInvariant();
            DataGridRow dgrSelectedRow = FindParent<DataGridRow>(sender as DependencyObject);
            Vendor selectedVendor = (Vendor)dgrSelectedRow.DataContext;

            if (source.GetType() == typeof(Image) && isAction == "DELETE")
            {
                DataAccess objDataAccess = new DataAccess();
                bool isVendorDeleted = objDataAccess.DeleteVendor(selectedVendor.VendorCode);
                if (isVendorDeleted)
                    MessageBox.Show("Vendor Deleted Successfully");
                else
                MessageBox.Show("Vendor Deletion Failed!!!");
                LoadVendors();
            }


            if (source.GetType() == typeof(Image) && isAction == "EDIT")
            {
                VendorScreen objVendorScreen = new VendorScreen();
                objVendorScreen.EditVendor = selectedVendor;
                objVendorScreen.DataChanged += ObjVendorScreen_DataChanged;
                objVendorScreen.ShowDialog();
            }
        }

        private void ObjVendorScreen_DataChanged(object sender, EventArgs e)
        {
            LoadVendors();
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

        private void cVendorFields_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cVendorFields.SelectedItem.ToString() == "Vendor Name")
            {
                lEquals.Content = "Like";
            }
            else
                lEquals.Content = "=";



        }
    }

}
