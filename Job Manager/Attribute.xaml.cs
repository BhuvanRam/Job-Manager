using Job_Manager.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
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
    /// Interaction logic for Attribute.xaml
    /// </summary>
    [PrincipalPermission(SecurityAction.Demand,Authenticated=false)]
    public partial class Attribute : Window,IView
    {
        public Attribute(AttributeViewModel attributeViewModel)
        {
            ViewModel = attributeViewModel;
            InitializeComponent();
        }

        public IViewModel ViewModel
        {
            get
            {
                return DataContext as IViewModel;
            }

            set
            {
                DataContext = value;
            }
        }
    }
}
