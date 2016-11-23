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
using System.Collections;

namespace Job_Manager
{
    /// <summary>
    /// Interaction logic for MaterialMainScreen.xaml
    /// </summary>
    public partial class MaterialMainScreen : Window
    {
        public MaterialMainScreen()
        {
            InitializeComponent();
            LoadMaterialAttributes();
        }

        public void LoadMaterialAttributes()
        {
            DataAccess objDataAccess = new DataAccess();
            List<MaterialAttribute> materialDataSource = objDataAccess.GetMaterialAttributes();                                    
            dMainMaterialGrid.ItemsSource = materialDataSource;
        }

        private void bAddMaterial_Click(object sender, RoutedEventArgs e)
        {
            MaterialScreen objMaterialScreen = new MaterialScreen();
            objMaterialScreen.ShowDialog();
        }
    }
}
