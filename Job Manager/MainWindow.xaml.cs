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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
    }
}
