using Job_Manager.ViewModel;
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
    /// Interaction logic for AttributeMainScreen.xaml
    /// </summary>
    public partial class AttributeMainScreen : Window
    {
        DataAccess objDataAccess = new DataAccess();
        public AttributeMainScreen()
        {
            InitializeComponent();
            GetGridData();
        }
        
        public void GetGridData()
        {
            var gridData = objDataAccess.GetAttributeGridDetails();
            dgAttributeGrid.ItemsSource = gridData;
            
        }

        private void bAddAttribute_Click(object sender, RoutedEventArgs e)
        {
            AttributeViewModel objAttributeViewModel = new AttributeViewModel();
            var attributeView = new Attribute(objAttributeViewModel);
            attributeView.ShowDialog();
        }
    }
}
