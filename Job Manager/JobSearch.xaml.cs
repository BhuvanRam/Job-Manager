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
    /// Interaction logic for JobSearch.xaml
    /// </summary>
    public partial class JobSearch : Window
    {
        DataAccess da = new DataAccess();
        public JobSearch()
        {
            InitializeComponent();
            LoadFilters();
        }
        private void LoadFilters()
        {
            List<JobManager.Model.BranchModel> branches = da.GetBranches();
            cmbBranch.DisplayMemberPath = "Name";
            cmbBranch.SelectedValuePath = "Id";
            cmbBranch.ItemsSource = branches;
            cmbBranch.SelectedValue = -1;

        }
        private void txtSearchJobId_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (((int)e.Key >= 34 && (int)e.Key <= 43) || (int)e.Key == 2 || (int)e.Key == 32 || (int)e.Key == 23 || (int)e.Key == 25)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            int jobId = 0;
            if ((this.txtSearchJobId.Text != string.Empty)&&(!Int32.TryParse(this.txtSearchJobId.Text, out jobId)))
            {
                MessageBox.Show("Please enter valid Job Id");
                this.txtSearchJobId.Focus();
                return;
            }
            if(dtPickEndDate.SelectedDate !=null && dtPickStartDate.SelectedDate != null)
            {
                if(dtPickEndDate.SelectedDate.Value <= dtPickStartDate.SelectedDate.Value)
                {
                    MessageBox.Show("End date should be greater than Start date");
                    this.dtPickEndDate.Focus();
                    return;
                }
            }
            int? jobIdParam =null,branchIdParam=null;
            if (jobId > 0)
            {
                jobIdParam = jobId;
            }
            if ((int)cmbBranch.SelectedValue != -1)
            {
                branchIdParam = (int)cmbBranch.SelectedValue;
            }
            List<JobModel> jobs = da.SearchJobs(jobIdParam, branchIdParam, dtPickStartDate.SelectedDate, dtPickEndDate.SelectedDate);
            dgJobSearchResult.ItemsSource = jobs;
        }
        private void DG_JobId_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink link = (Hyperlink)e.OriginalSource;
            int jobId = ((JobManager.Model.JobModel)((System.Windows.FrameworkContentElement)e.OriginalSource).DataContext).JobId;
            JobDetails jobDetails = new Job_Manager.JobDetails(jobId);
            jobDetails.ShowInTaskbar = false;
            jobDetails.Owner = this;
            jobDetails.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            jobDetails.ShowDialog();
        }
    }
}
