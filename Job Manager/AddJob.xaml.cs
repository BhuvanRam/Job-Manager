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
using System.Threading;
using JobManager.DAL;
using Job_Manager.ViewModel;

namespace Job_Manager
{
    /// <summary>
    /// Interaction logic for AddJob.xaml
    /// </summary>
    public partial class AddJob : Window
    {
        DataAccess objDataAccess = new DataAccess();
        public AddJob()
        {
            InitializeComponent();
            this.txtCreatedOn.Text = DateTime.Now.ToShortDateString();
            UserIdentity.CustomPrincipal customPrincipal = Thread.CurrentPrincipal as UserIdentity.CustomPrincipal;
            this.txtCreatedBy.Text = customPrincipal.Identity.Name;

            List<JobManager.Model.BranchModel> branches = objDataAccess.GetBranches();
            cmbBranch.DisplayMemberPath = "Name";
            cmbBranch.SelectedValuePath = "Id";
            cmbBranch.ItemsSource = branches;
            cmbBranch.SelectedValue = -1;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(Convert.ToInt32(cmbBranch.SelectedValue) == -1)
            {
                MessageBox.Show("Please select Branch");
                cmbBranch.Focus();
                return;
            }
            Job objJob = new Job()
            {
                JobName = txtJobName.Text,
                CreatedBy = txtCreatedBy.Text,
                CreatedDate = Convert.ToDateTime(txtCreatedOn.Text),
                BranchId = Convert.ToInt32(cmbBranch.SelectedValue),    
                StatusId = 1        
            };

            int newJobId = objDataAccess.AddJob(objJob);
            if (newJobId > 0 )
            {
                MessageBox.Show("Job Id:"+newJobId+" added successfully");
                JobDetails jobDetails = new Job_Manager.JobDetails(newJobId);
                jobDetails.ShowInTaskbar = false;
                jobDetails.Owner = this.Owner;
                jobDetails.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                this.Close();
                jobDetails.ShowDialog();                
            }
            else
                MessageBox.Show("Failed to Add Job");
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
