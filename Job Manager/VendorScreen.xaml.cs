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
    /// Interaction logic for Vendor.xaml
    /// </summary>
    public partial class VendorScreen : Window
    {

        DataAccess objDataAccess = new DataAccess();
        private Vendor _editVendor;
        private List<State> _listOfStates;

        public delegate void DataChangedEventHandler(object sender, EventArgs e);
        public event DataChangedEventHandler DataChanged;

        public VendorScreen()
        {
            InitializeComponent();
            LoadStates();

        }

        public Vendor EditVendor
        {
            get
            {
                return _editVendor;
            }

            set
            {
                _editVendor = value;
                SetEditFields();
      
            }
        }

        public List<State> ListOfStates
        {
            get
            {
                return _listOfStates;
            }

            set
            {
                _listOfStates = value;
            }
        }

        public void SetEditFields()
        {
            txtVendorCode.Text = _editVendor.VendorCode;
            txtVendorName.Text = _editVendor.VendorName;
            txtAddress1.Text = _editVendor.Address1;
            txtAddress2.Text = _editVendor.Address2;
            txtCity.Text = _editVendor.City;
            cState.SelectedItem = ListOfStates.Where(p => p.StateID == _editVendor.StateId).Select(p => p).FirstOrDefault();
            txtPin.Text = _editVendor.PIN;
            txtPhone.Text = _editVendor.PhoneNo;
            txtFax.Text = _editVendor.FaxNo;
            txtEmail.Text = _editVendor.Email;
            txtContactPerson.Text = _editVendor.ContactPerson;
            txtVendorCode.IsEnabled = false;
        }

        public void LoadStates()
        {
            ListOfStates =  objDataAccess.GetStates();
            cState.ItemsSource = ListOfStates;            
        }

        private void btnAddVendor_Click(object sender, RoutedEventArgs e)
        {
            Vendor objVendor = objDataAccess.GetVendor(txtVendorCode.Text);
            State selectedState = (State)cState.SelectedItem;

      
            if (objVendor == null)
            {
                Vendor objInsertVendor = new Vendor()
                {
                    VendorCode = txtVendorCode.Text,
                    VendorName = txtVendorName.Text,
                    Address1 = txtAddress1.Text,
                    Address2 = txtAddress2.Text,
                    City = txtCity.Text,
                    StateId = selectedState.StateID,
                    PIN = txtPin.Text,
                    PhoneNo = txtPhone.Text,
                    FaxNo = txtFax.Text,
                    Email = txtEmail.Text,
                    ContactPerson = txtContactPerson.Text
                };

                bool isInserted = objDataAccess.AddVendor(objInsertVendor);
                if (isInserted)
                {
                    MessageBox.Show("Vendor Added successfully");
                    ClearScreen();
                }
                else
                    MessageBox.Show("Vendor Failed to Add");

            }
            else
            {
                if (EditVendor != null)
                {
                    objVendor.VendorCode = txtVendorCode.Text;
                    objVendor.VendorName = txtVendorName.Text;
                    objVendor.Address1 = txtAddress1.Text;
                    objVendor.Address2 = txtAddress2.Text;
                    objVendor.City = txtCity.Text;
                    objVendor.StateId = selectedState.StateID;
                    objVendor.PIN = txtPin.Text;
                    objVendor.PhoneNo = txtPhone.Text;
                    objVendor.FaxNo = txtFax.Text;
                    objVendor.Email = txtEmail.Text;
                    objVendor.ContactPerson = txtContactPerson.Text;
                    bool isUpdated = objDataAccess.UpdateVendor(objVendor);
                    if (isUpdated)
                        MessageBox.Show("Vendor Updated successfully");
                    else
                        MessageBox.Show("Vendor Update Failed!!! ");
                }
                else
                    MessageBox.Show("Vendor ID Already exists!!! Enter another Vendor ID");
            }

            DataChanged?.Invoke(this, new EventArgs());

        }

        private void btnClearVendor_Click(object sender, RoutedEventArgs e)
        {
            ClearScreen();
        }

       
        public void ClearScreen()
        {
            txtVendorCode.Clear();
            txtVendorName.Clear();
            txtAddress1.Clear();
            txtAddress2.Clear();
            txtCity.Clear();
            cState.SelectedItem = null;
            txtPin.Clear();
            txtPhone.Clear();
            txtFax.Clear();
            txtEmail.Clear();
            txtContactPerson.Clear();
            txtVendorCode.IsEnabled = true;
        }
    }
}
