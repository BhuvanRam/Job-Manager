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
using JobManager.DAL;
using JobManager.Model;

namespace Job_Manager
{
    /// <summary>
    /// Interaction logic for CreatePOfromJob.xaml
    /// </summary>
    public partial class CreatePOfromJob : Window
    {
        int jobId;
        int POId;
        JobPOModel jobPO;
        DataAccess da = new DataAccess();
        List<JobMaterialModel> lJobMaterials;
        public CreatePOfromJob(int jobId)
        {
            this.jobId = jobId;
            InitializeComponent();
            LoadJobPODetails();
        }
        public CreatePOfromJob(int jobId, List<JobMaterialModel> JobMaterials)
        {
            this.lJobMaterials = JobMaterials;
            this.jobId = jobId;
            InitializeComponent();
            LoadJobPODetails();
        }
        public CreatePOfromJob(int jobId,int POId)
        {
            this.jobId = jobId;
            this.POId = POId;
            InitializeComponent();
            LoadJobPODetails();
        }

        public void LoadJobPODetails()
        {

           List<Vendor> vendors=  da.GetAllVendors();

            Vendor selectitem = new Vendor();
            selectitem.VendorId = -1;
            selectitem.VendorName = "<Select>";
            vendors.Insert(0, selectitem);     
             
            cmbVendor.DisplayMemberPath = "VendorName";
            cmbVendor.SelectedValuePath = "VendorId";
            cmbVendor.ItemsSource = vendors;
            cmbVendor.SelectedValue = -1;

            List<JobPOStatus> js = da.GetJobPOStatuses();
            cmbStatus.DisplayMemberPath = "Name";
            cmbStatus.SelectedValuePath = "Id";
            cmbStatus.ItemsSource = js;
            cmbStatus.SelectedValue = 1;

            List<JobPOMaterialModel> materials = new List<JobPOMaterialModel>();

            if (POId != 0)
            {
                jobPO = da.GetJobPODetails(POId);
                this.txtPONumber.Text = jobPO.PONumber;
                this.txtDiscount.Text = jobPO.Discount.ToString();
                this.txtDelivery.Text = jobPO.Delivery.ToString();
                this.txtPayment.Text = jobPO.Payment.ToString();
                this.txtPacking.Text = jobPO.Packing.ToString();
                this.txtExciseDuty.Text = jobPO.ExciseDuty.ToString();
                this.txtTaxesExtra.Text = jobPO.TaxesExtra.ToString();
                this.txtTransitInsurance.Text = jobPO.TransportInsurance.ToString();
                this.txtTransportation.Text = jobPO.Transportation.ToString();
                this.txtOctroi.Text = jobPO.Octroi.ToString();
                this.cmbVendor.SelectedValue = jobPO.VendorId;
                this.txtCreatedBy.Text = jobPO.CreatedBy;
                this.txtCreatedOn.Text = jobPO.CreatedOn.ToString();
                this.txtApprovedBy.Text = jobPO.ApprovedBy;
                if (jobPO.ApprovedOn.HasValue)
                    this.txtApprovedOn.Text = jobPO.ApprovedOn.ToString();
                cmbStatus.SelectedValue = jobPO.StatusId;
                materials = da.GetJobPOMaterials(jobId, POId);
            }

            else
            {

                List<JobPOMaterialModel> allMaterials = da.GetJobPOMaterials(jobId, null);

                foreach(JobPOMaterialModel jobpom in allMaterials)
                {
                    if(lJobMaterials.Find(p => p.Id == jobpom.Id) != null)
                    {
                        materials.Add(jobpom);
                    }
                }
            }
               
            dgJobMaterials.ItemsSource = materials;            
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(Convert.ToInt32(cmbVendor.SelectedValue) == -1)
            {
                MessageBox.Show("Please select Vendor", "Validation");
                this.cmbVendor.Focus();
                return;
            }
            if(this.jobPO == null)
                jobPO = new JobPOModel();
            jobPO.VendorId = Convert.ToInt32(this.cmbVendor.SelectedValue);
            jobPO.JobId = jobId;
            jobPO.CreatedById = 1; // TODO check with Bhuvan how to get logged in user id
            jobPO.StatusId = Convert.ToInt32(this.cmbStatus.SelectedValue);
            if(this.txtDiscount.Text != "")
                jobPO.Discount = Convert.ToDecimal(this.txtDiscount.Text);
            if(this.txtDelivery.Text != "")
                jobPO.Delivery = Convert.ToDecimal(this.txtDelivery.Text);
            if (this.txtPayment.Text != "")
                jobPO.Payment = Convert.ToDecimal(this.txtPayment.Text);
            if (this.txtPacking.Text != "")
                jobPO.Packing = Convert.ToDecimal(this.txtPacking.Text);
            if (this.txtExciseDuty.Text != "")
                jobPO.ExciseDuty = Convert.ToDecimal(this.txtExciseDuty.Text);
            if (this.txtTaxesExtra.Text != "")
                jobPO.TaxesExtra = Convert.ToDecimal(this.txtTaxesExtra.Text);
            if (this.txtTransitInsurance.Text != "")
                jobPO.TransportInsurance = Convert.ToDecimal(this.txtTransitInsurance.Text);
            if (this.txtTransportation.Text != "")
                jobPO.Transportation = Convert.ToDecimal(this.txtTransportation.Text);
            if (this.txtOctroi.Text != "")
                jobPO.Octroi = Convert.ToDecimal(this.txtOctroi.Text);
            if(jobPO.ApprovedById == null && jobPO.StatusId == 2)
            {
                jobPO.ApprovedById = 1; // TODO check with Bhuvan how to get logged in user id
                jobPO.ApprovedOn = DateTime.Now;
            }
            if (POId == 0)
            {
                POId = da.InsJobPO(jobPO);

                foreach (JobPOMaterialModel jpm in dgJobMaterials.Items)
                {
                    da.InsJobPOMaterila(POId, jobId, jpm);
                }
            }else
            {
                jobPO.ModifiedById = 1; //TODO check with Bhuvan how to get logged in user id
                da.DelPOMaterials(POId);
                da.UpdJobPO(jobPO);
                foreach (JobPOMaterialModel jpm in dgJobMaterials.Items)
                {
                    da.InsJobPOMaterila(POId, jobId, jpm);
                }
            }
            LoadJobPODetails();
            MessageBox.Show("Purchase Order Saved Sucessfully", "Information");
            var myObject = this.Owner as JobDetails;
            myObject.LoadJobDetails();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void dgJobMaterials_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            if (dg != null)
            {
                DataGridRow dgr = (DataGridRow)(dg.ItemContainerGenerator.ContainerFromIndex(dg.SelectedIndex));
                if (e.Key == Key.Delete && !(e.Device.Target.GetType().Name == "TextBox"))
                {
                    if(dg.Items.Count == 1)
                    {
                        MessageBox.Show("Cannot delete this material. At least one material is required in Purchase Order.","Information");
                        e.Handled = true;
                        return;
                    }
                    // User is attempting to delete the row
                    var result = MessageBox.Show(
                        "About to delete the current material from Purchase Order.\n\nProceed?",
                        "Delete",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question,
                        MessageBoxResult.No);
                    e.Handled = (result == MessageBoxResult.No);
                }
            }


        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (POId != 0)
                {
                    PurchaseOrderReport poReport = new PurchaseOrderReport(POId);
                    poReport.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error While generating Report");
                

            }
            
        }
    }
}
