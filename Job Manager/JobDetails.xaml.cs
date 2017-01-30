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
using System.Windows.Navigation;
using System.Windows.Shapes;
using JobManager.DAL;
using JobManager.Model;
using System.Security.Permissions;

namespace Job_Manager
{
    /// <summary>
    /// Verifying the GIT checkin
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [PrincipalPermission(SecurityAction.Demand, Role = "Administrators")]
    public partial class JobDetails : Window
    {
        private int jobId;
        DataAccess da = new DataAccess();
        JobModel job;
        JobMaterialModel jm;
        public JobDetails()
        {
            InitializeComponent();

            //jobId = 1;lbl
            //LoadJobDetails();            
            this.TopCanvas.Visibility = Visibility.Hidden;
            this.dgJobMaterials.Visibility = Visibility.Hidden;
            this.lblErrorMsg.Visibility = Visibility.Hidden;
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            jobId = Int32.Parse(this.txtSearchJobId.Text);
            job = da.GetJobDetails(jobId);
            if (job != null)
            {
                LoadJobDetails();
                this.txtJobId.Text = job.JobId.ToString();
                this.txtJobName.Text = job.JobName;
                this.txtCreateDate.Text = job.CreatedDate.ToShortDateString();
                this.cmbStatus.SelectedValue = job.StatusId;
                this.TopCanvas.Visibility = Visibility.Visible;
                this.dgJobMaterials.Visibility = Visibility.Visible;
                this.lblErrorMsg.Visibility = Visibility.Hidden;
            }
            else
            {
                this.TopCanvas.Visibility = Visibility.Hidden;
                this.dgJobMaterials.Visibility = Visibility.Hidden;
                this.lblErrorMsg.Visibility = Visibility.Visible;
            }
        }
        public void LoadJobDetails()    
        {
            JobModel jobModel = da.GetJobMaterials(jobId);
            dgJobMaterials.ItemsSource = jobModel.Materials;

            List<JobStatus> js = da.GetJobStatuses();
            cmbStatus.DisplayMemberPath = "Name";
            cmbStatus.SelectedValuePath = "Id";
            cmbStatus.ItemsSource = js;
        }
        private void btnAddMaterial_Click(object sender, RoutedEventArgs e)
        {
            AddMaterialToJob addMaterialWindow = new Job_Manager.AddMaterialToJob(jobId);
            addMaterialWindow.ShowInTaskbar = false;
            addMaterialWindow.Owner = this;
            addMaterialWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            addMaterialWindow.ShowDialog();
        }

        private void btnDeleteMaterial_Click(object sender, RoutedEventArgs e)
        {
            JobMaterialModel material = (JobMaterialModel)this.dgJobMaterials.SelectedItem;
            da.DeleteJobMaterial(jobId, material.Id);
            MessageBox.Show("Selected Material Successfully deleted from the Job");

            LoadJobDetails();
        }

        private void txtSearchJobId_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = (int)e.Key >= 43 || (int)e.Key <= 34;
        }

        private void dgJobMaterials_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgJobMaterials.SelectedItems.Count > 1)
            {
               // this.btnEditMaterial.IsEnabled = false;
                jm = null;
            }
            else
            {
                jm = (JobMaterialModel)this.dgJobMaterials.SelectedItem;
                //this.btnEditMaterial.IsEnabled = true;
            }
        }

        private void btnCreatePO_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddJob_Click(object sender, RoutedEventArgs e)
        {
            AddJob addJob = new Job_Manager.AddJob();
            addJob.ShowInTaskbar = false;
            addJob.Owner = this;
            addJob.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            addJob.ShowDialog();
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                job.JobName = this.txtJobName.Text;
                job.StatusId = Convert.ToInt32(this.cmbStatus.SelectedValue);
                da.SaveJobDetails(job);
                MessageBox.Show("Job details Updated Successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
