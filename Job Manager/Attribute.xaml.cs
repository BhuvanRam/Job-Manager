using Job_Manager.ViewModel;
using JobManager.Model;
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
    [PrincipalPermission(SecurityAction.Demand, Authenticated = false)]
    public partial class Attribute : Window, IView
    {
        public Attribute(AttributeViewModel attributeViewModel)
        {
            ViewModel = attributeViewModel;
            InitializeComponent();
            LoadAttributes();
            LoadAttributeTypes();
            ddlParentAttributes.Visibility = Visibility.Hidden;
            dGridNewAttributes.Visibility = Visibility.Hidden;
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

        private void LoadAttributes()
        {
            JobManager.DAL.DataAccess objDataAccess = new JobManager.DAL.DataAccess();
            List<AttributeModel> lAttributeModels = objDataAccess.GetAllAttributes();
            dAttributeName.ItemsSource = lAttributeModels;
        }

        private void LoadAttributeTypes()
        {
            JobManager.DAL.DataAccess objDataAccess = new JobManager.DAL.DataAccess();
            List<AttributeTypeModel> lAttributeTypeModels = objDataAccess.GetAllAttributeTypes();
            ddlAttributeType.ItemsSource = lAttributeTypeModels;
        }

        private void ddlAttributeType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            JobManager.DAL.DataAccess objDataAccess = new JobManager.DAL.DataAccess();


            AttributeTypeModel objAttributeTypeModel = (AttributeTypeModel)((ComboBox)sender).SelectedItem;

            if (objAttributeTypeModel.AttributeTypeId == 2)
            {
                lblParentAttribute.Content = "Parent Attribute";
                List<AttributeModel> lAttributeModels = objDataAccess.GetAttributeByType(2);
                AttributeModel objAttributeModel = new AttributeModel();
                ddlParentAttributes.DisplayMemberPath = nameof(objAttributeModel.AttributeName);
                ddlParentAttributes.SelectedValuePath = nameof(objAttributeModel.AttributeId);
                ddlParentAttributes.ItemsSource = lAttributeModels;

                if (dAttributeName.SelectedValue != null) // New Value
                {
                    // Assign Parent Value Here
                    AttributeModel objParentAttributeModel = objDataAccess.GetAttributeByAttributeId(Convert.ToInt32(dAttributeName.SelectedValue));
                    if (objParentAttributeModel.ParentId != 0)
                        ddlParentAttributes.SelectedValue = objParentAttributeModel.ParentId;
                    else
                    {
                        // Clear Grid and load Attribute Values
                    }
                }
                else
                {
                    List<NewAttributeGrid> lNewAttributeGrid = new List<NewAttributeGrid>();
                    dGridNewAttributes.ItemsSource = lNewAttributeGrid;
                }


                ddlParentAttributes.Visibility = Visibility.Visible;
                dGridNewAttributes.Visibility = Visibility.Visible;
                txtParentAttribute.Visibility = Visibility.Collapsed;
            }
            else
            {
                lblParentAttribute.Content = "Attribute Value";
                txtParentAttribute.Visibility = Visibility.Visible;
                ddlParentAttributes.Visibility = Visibility.Collapsed;
                dGridNewAttributes.Visibility = Visibility.Collapsed;
            }
        }

        private void ddlParentAttributes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dAttributeName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmbAttribute = (ComboBox)sender;
            if (cmbAttribute.SelectedItem != null)
            {
                AttributeModel objAttributeModel = (AttributeModel)cmbAttribute.SelectedItem;
                int iSelectedAttributeValue = Convert.ToInt32(objAttributeModel.AttributeTypeId);
                JobManager.DAL.DataAccess objDataAccess = new JobManager.DAL.DataAccess();
                List<AttributeTypeModel> lAttributeTypeModel = objDataAccess.GetAllAttributeTypes();
                AttributeTypeModel selectedType = lAttributeTypeModel.Where(p => p.AttributeTypeId == iSelectedAttributeValue).SingleOrDefault();
                //selected changed event doesnt fires if the value is same          
                ddlAttributeType.SelectedValue = selectedType.AttributeTypeId.ToString();
                if (objAttributeModel != null)
                {
                    int iAttributeId = objAttributeModel.AttributeId;
                    List<AttributeValueModel> lAttributeValueModel = objDataAccess.GetAttributeValueByAttributeId(iAttributeId);
                    LoadGrid(lAttributeValueModel);

                    AttributeModel objParentAttributeModel = objDataAccess.GetAttributeByAttributeId(iAttributeId);
                    if (objParentAttributeModel.ParentId != 0)
                        ddlParentAttributes.SelectedValue = objParentAttributeModel.ParentId;
                    else
                        ddlParentAttributes.SelectedValue = null;
                }
            }
            else
            {
                List<NewAttributeGrid> lNewAttributeGrid = new List<NewAttributeGrid>();
                dGridNewAttributes.ItemsSource = lNewAttributeGrid;
            }

        }

        public void LoadGrid(List<AttributeValueModel> lParentAttributeValueModel)
        {
            JobManager.DAL.DataAccess objDataAccess = new JobManager.DAL.DataAccess();
            List<NewAttributeGrid> lNewAttributeGrid = new List<NewAttributeGrid>();

            foreach (var item in lParentAttributeValueModel)
            {
                List<AttributeValueModel> lsubAttributeValueModel = new List<AttributeValueModel>();
                lsubAttributeValueModel = objDataAccess.GetAttributeValueByParentId(item.Id);

                string childAttributeCsvValue = string.Empty;

                AttributeModel objParentofSubAttribute = new AttributeModel();
                lsubAttributeValueModel.ForEach(p =>
               {
                   string sComma = string.IsNullOrEmpty(childAttributeCsvValue) ? " " : ",";
                   childAttributeCsvValue = childAttributeCsvValue + sComma + p.Name;
                   if (p.AttributeId != 0)
                       objParentofSubAttribute = objDataAccess.GetAttributeByAttributeId(p.AttributeId);
               });


                lNewAttributeGrid.Add(new NewAttributeGrid()
                {
                    ParentAttributeValueId = item.Id,
                    ParentValue = item.Name,
                    CurrentAttribute = childAttributeCsvValue,
                    CurrentAttributeToCheckModification = childAttributeCsvValue,
                    ParentOfSubAttribute = objParentofSubAttribute
                });
            }
            dGridNewAttributes.ItemsSource = lNewAttributeGrid;

            List<AttributeModel> lAttributeNames = (List<AttributeModel>)dAttributeName.ItemsSource;
            int iselectedAttributeId = Convert.ToInt32(dAttributeName.SelectedValue);
            List<AttributeModel> FilteredAttributes = lAttributeNames.Where(p => p.AttributeId != iselectedAttributeId).Select(p => p).ToList();
            Parent_SubAtributeValue.ItemsSource = FilteredAttributes;
        }


        private void bAddAttribute_Click(object sender, RoutedEventArgs e)
        {
            JobManager.DAL.DataAccess objDataAccess = new JobManager.DAL.DataAccess();

            // 1. Attribute Insertion

            int iAttributeId = 0;
            int iAttributeTypeSelectedValue = Convert.ToInt32(ddlAttributeType.SelectedValue);
            int iParentId = Convert.ToInt32(ddlParentAttributes.SelectedValue);
            if (dAttributeName.SelectedValue == null)
            {
                AttributeModel objAttributeModel = new AttributeModel();
                objAttributeModel.AttributeName = dAttributeName.Text;
                objAttributeModel.AttributeTypeId = iAttributeTypeSelectedValue;
                objAttributeModel.ParentId = iParentId;
                objDataAccess.AddAttribute(objAttributeModel);
                iAttributeId = objAttributeModel.AttributeId;
                MessageBox.Show("Attribute Added Successfully");
                LoadAttributes();
            }
            else iAttributeId = Convert.ToInt32(dAttributeName.SelectedValue);

            if (iAttributeId != 0)
            {
                // if user selects "select option"
                if (iAttributeTypeSelectedValue == 2)
                {
                    List<NewAttributeGrid> lAttributeGrid = new List<NewAttributeGrid>();
                    lAttributeGrid = (List<NewAttributeGrid>)dGridNewAttributes.ItemsSource;
                    List<AttributeValueModel> lAttributeValueModel = new List<AttributeValueModel>();

                    // 2. Parent Column Insertion
                    foreach (NewAttributeGrid itemNewAttributeGrid in lAttributeGrid)
                    {
                        if(itemNewAttributeGrid.ParentAttributeValueId ==0)
                        {
                            AttributeValueModel objAttributeValueModel = new AttributeValueModel();
                            objAttributeValueModel.Name = itemNewAttributeGrid.ParentValue;                            
                            objAttributeValueModel.AttributeId = iAttributeId;

                            // Insert into Database  and assign the value to Grid again
                            objAttributeValueModel = objDataAccess.AddAttributeValue(objAttributeValueModel);                            
                            itemNewAttributeGrid.ParentAttributeValueId = objAttributeValueModel.Id;
                        }
                    }


                    /* Check whether the row is modified or not*/
                    foreach (NewAttributeGrid itemNewAttributeGrid in lAttributeGrid)
                    {
                        bool bIsmodified = (itemNewAttributeGrid.CurrentAttribute != itemNewAttributeGrid.CurrentAttributeToCheckModification);
                        //bool bIsContains = itemNewAttributeGrid.CurrentAttribute.Contains(itemNewAttributeGrid.CurrentAttributeToCheckModification);
                        if (bIsmodified)
                        {
                            string sNewAttributeValue = string.Empty;
                            if (!string.IsNullOrEmpty(itemNewAttributeGrid.CurrentAttributeToCheckModification))
                                sNewAttributeValue = itemNewAttributeGrid.CurrentAttribute.Replace(itemNewAttributeGrid.CurrentAttributeToCheckModification, string.Empty);
                            else
                                sNewAttributeValue = itemNewAttributeGrid.CurrentAttribute;


                            string[] sNewAttributes = sNewAttributeValue.Split(',');
                            int iSubAttributeValueAttributeId = itemNewAttributeGrid.ParentOfSubAttribute.AttributeId;
                            foreach (string newAttribute in sNewAttributes)
                            {
                                AttributeValueModel objAttributeValueModel = new AttributeValueModel();
                                objAttributeValueModel.Name = newAttribute;
                                objAttributeValueModel.ParentValue = itemNewAttributeGrid.ParentAttributeValueId;
                                objAttributeValueModel.AttributeId = iSubAttributeValueAttributeId;
                                lAttributeValueModel.Add(objAttributeValueModel);
                            }

                            // 1. Saving Attribute Value Model
                            bool isResult = objDataAccess.AddAttributeValueList(lAttributeValueModel);
                            if (isResult) MessageBox.Show("Attribute Values inserted successfully");
                            else MessageBox.Show("Attribute Values insertion failed");
                        }


                    }

                    /* If the Row is not modified then add it to Stagnate List */
                    /* If the Row is modified then respond accordingly */


                }
            }

            else if (iAttributeTypeSelectedValue == 1)
            {
                string sAttributeValue = txtParentAttribute.Text;
                objDataAccess.AddAttributeValueForPlainText(sAttributeValue, iAttributeId);

            }
        }

        /*
        public void AttriuteChangedEvent(int AttributeTypeId)
        {
            JobManager.DAL.DataAccess objDataAccess = new JobManager.DAL.DataAccess();
            if (AttributeTypeId == 2)
            {
                List<AttributeModel> lAttributeModels = objDataAccess.GetAttributeByType(2);
                AttributeModel objAttributeModel = new AttributeModel();
                ddlParentAttributes.DisplayMemberPath = nameof(objAttributeModel.AttributeName);
                ddlParentAttributes.SelectedValuePath = nameof(objAttributeModel.AttributeId);
                ddlParentAttributes.ItemsSource = lAttributeModels;

                // Assign Parent Value Here
                AttributeModel objParentAttributeModel = objDataAccess.GetAttributeByAttributeId(Convert.ToInt32(dAttributeName.SelectedValue));
                if (objParentAttributeModel.ParentId != 0)
                    ddlParentAttributes.SelectedValue = objParentAttributeModel.ParentId;
                else
                {
                    // Clear Grid and load Attribute Values
                    dGridNewAttributes.ItemsSource = null;


                }

                ddlParentAttributes.Visibility = Visibility.Visible;
                dGridNewAttributes.Visibility = Visibility.Visible;
                txtParentAttribute.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtParentAttribute.Visibility = Visibility.Visible;
                ddlParentAttributes.Visibility = Visibility.Collapsed;
                dGridNewAttributes.Visibility = Visibility.Collapsed;
            }
        }
        */


        private class NewAttributeGrid
        {
            public int ParentAttributeValueId { get; set; }
            public string ParentValue { get; set; }
            public string CurrentAttribute { get; set; }
            public string CurrentAttributeToCheckModification { get; set; }
            public string CurrentAttributeCloneId { get; set; }
            public AttributeModel ParentOfSubAttribute { get; set; }
        }

    }

 
}
