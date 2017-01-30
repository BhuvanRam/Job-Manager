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
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Job objJob = new Job()
            {
                JobName = txtJobName.Text,
                CreatedBy = txtCreatedBy.Text,
                CreatedDate = Convert.ToDateTime(txtCreatedOn.Text)             
            };

            bool isInserted = objDataAccess.AddJob(objJob);
            if (isInserted)
            {
                MessageBox.Show("Job Added successfully");
                this.Close();
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
