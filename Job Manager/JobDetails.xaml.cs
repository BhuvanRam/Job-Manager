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

        }
        public JobDetails(int jobId)
        {
            InitializeComponent();

            this.jobId = jobId;
            LoadJobDetails();            
        }
        //public void btnSearch_Click(object sender, RoutedEventArgs e)
        //{
        //    if (Int32.TryParse(this.txtSearchJobId.Text, out jobId)){
        //        job = da.GetJobDetails(jobId);
        //        if (job != null)
        //        {
        //            LoadJobDetails();
        //            this.txtJobId.Text = job.JobId.ToString();
        //            this.txtJobName.Text = job.JobName;
        //            this.txtCreateDate.Text = job.CreatedDate.ToShortDateString();
        //            this.cmbStatus.SelectedValue = job.StatusId;
        //            if (job.BranchId != null)
        //                this.cmbBranch.SelectedValue = job.BranchId;
        //            this.TopCanvas.Visibility = Visibility.Visible;
        //            this.dgJobMaterials.Visibility = Visibility.Visible;
        //            this.lblErrorMsg.Visibility = Visibility.Hidden;
        //        }
        //        else
        //        {
        //            this.TopCanvas.Visibility = Visibility.Hidden;
        //            this.dgJobMaterials.Visibility = Visibility.Hidden;
        //            this.lblErrorMsg.Visibility = Visibility.Visible;
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Please enter valid Job Id");
        //        this.txtSearchJobId.Focus();
        //    }
        //}
        public void LoadJobDetails()    
        {
            job = da.GetJobDetails(jobId);
            List<JobManager.Model.JobStatus> js = da.GetJobStatuses();
            cmbStatus.DisplayMemberPath = "Name";
            cmbStatus.SelectedValuePath = "Id";
            cmbStatus.ItemsSource = js;

            List<JobManager.Model.BranchModel> branches = da.GetBranches();
            cmbBranch.DisplayMemberPath = "Name";
            cmbBranch.SelectedValuePath = "Id";
            cmbBranch.ItemsSource = branches;
            cmbBranch.SelectedValue = -1;

            this.txtJobId.Text = job.JobId.ToString();
            this.txtJobName.Text = job.JobName;
            this.txtCreateDate.Text = job.CreatedDate.ToShortDateString();
            this.cmbStatus.SelectedValue = job.StatusId;
            if (job.BranchId != null)
                this.cmbBranch.SelectedValue = job.BranchId;

            JobModel jobModel = da.GetJobMaterials(jobId);
            dgJobMaterials.ItemsSource = jobModel.Materials;            

            if (job.StatusId == 2)
                this.btnCreatePO.IsEnabled = true;
            else
                this.btnCreatePO.IsEnabled = false;
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
            if(((int)e.Key >=34 && (int)e.Key <= 43) || (int)e.Key == 2 || (int)e.Key == 32 || (int)e.Key == 23 || (int)e.Key == 25)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void dgJobMaterials_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgJobMaterials.SelectedItems.Count > 1)
            {
                this.btnEditPO.IsEnabled = false;
                jm = null;
            }
            else
            {
                jm = (JobMaterialModel)this.dgJobMaterials.SelectedItem;
                if (jm !=null && jm.POId != null)
                {
                    this.btnEditPO.IsEnabled = true;
                }else
                {
                    this.btnEditPO.IsEnabled = false;
                }
            }
        }

        private void btnCreatePO_Click(object sender, RoutedEventArgs e)
        {
            List<JobMaterialModel> lJobMaterialModels = (List<JobMaterialModel>)dgJobMaterials.ItemsSource;
            List<JobMaterialModel> lJobMaterialIds = lJobMaterialModels.Where(p => p.IsSelected == true).Select(p => p).ToList();
            CreatePOfromJob createPO = new Job_Manager.CreatePOfromJob(this.jobId,lJobMaterialIds);
            createPO.ShowInTaskbar = false;
            createPO.Owner = this;
            createPO.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            createPO.ShowDialog();
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
                if(Convert.ToInt32(cmbBranch.SelectedValue) != -1)
                    job.BranchId = Convert.ToInt32(this.cmbBranch.SelectedValue);
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

        private void btnEditPO_Click(object sender, RoutedEventArgs e)
        {
            CreatePOfromJob createPO = new Job_Manager.CreatePOfromJob(this.jobId,Convert.ToInt32(jm.POId));
            createPO.ShowInTaskbar = false;
            createPO.Owner = this;
            createPO.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            createPO.ShowDialog();
        }
    }
}
