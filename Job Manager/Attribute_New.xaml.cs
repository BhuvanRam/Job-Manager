using JobManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Job_Manager
{
    /// <summary>
    /// Interaction logic for Attribute_New.xaml
    /// </summary>
    public partial class Attribute_New : Window
    {
        JobManager.DAL.DataAccess objDataAccess = new JobManager.DAL.DataAccess();
        public Attribute_New()
        {
            InitializeComponent();
            LoadAttributes();
            LoadAttributeTypes();
            ddlParentAttributes.Visibility = Visibility.Collapsed;
            dGridNewAttributes.Visibility = Visibility.Collapsed;
            bClear.Visibility = Visibility.Collapsed;
        }


        public void LoadAttributes()
        {
            List<AttributeModel> lAttributeModels = objDataAccess.GetAllAttributes();
            dAttributeName.ItemsSource = null;
            dAttributeName.ItemsSource = lAttributeModels;
        }

        private void LoadAttributeTypes()
        {
            List<AttributeTypeModel> lAttributeTypeModels = objDataAccess.GetAllAttributeTypes();
            ddlAttributeType.ItemsSource = lAttributeTypeModels;
        }

        private void ddlAttributeType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            JobManager.DAL.DataAccess objDataAccess = new JobManager.DAL.DataAccess();
            AttributeTypeModel objAttributeTypeModel = (AttributeTypeModel)((ComboBox)sender).SelectedItem;


            if (objAttributeTypeModel != null && objAttributeTypeModel.AttributeTypeId == 2)
            {
                List<AttributeValueModel> lNewAttributeGrid = new List<AttributeValueModel>();
                dGridNewAttributes.ItemsSource = lNewAttributeGrid;
                ParentDropdownSelectVisibilty();
                FillParentAttributeDropdown();
                GridParentDropdownSelectColumnsVisibility();
            }
            else
            {
                PlainTextSelectVisibility();
            }
        }

        private void FillParentAttributeDropdown()
        {
            List<AttributeModel> lAttributeModel = new List<AttributeModel>();
            if (dAttributeName.SelectedItem != null)
            {
                AttributeModel objAttributeModel = (AttributeModel)dAttributeName.SelectedItem;
                var anonModel = (List<AttributeModel>)dAttributeName.ItemsSource;
                lAttributeModel = anonModel.Where(p => p.AttributeId != objAttributeModel.AttributeId).Select(p => p).ToList<AttributeModel>();
                ddlParentAttributes.ItemsSource = lAttributeModel;
            }
            else
            {
                lAttributeModel = (List<AttributeModel>)dAttributeName.ItemsSource;
                if (lAttributeModel != null && lAttributeModel.Count() <= 0)
                    lAttributeModel = objDataAccess.GetAllAttributes();
                ddlParentAttributes.ItemsSource = lAttributeModel;
            }
        }

        private void ParentDropdownSelectVisibilty()
        {
            lblParentAttribute.Content = "Parent Attribute";
            ddlParentAttributes.Visibility = Visibility.Visible;
            ddlParentAttributes.SelectedItem = null;
            dGridNewAttributes.Visibility = Visibility.Visible;
            txtParentAttribute.Visibility = Visibility.Collapsed;
            bClear.Visibility = Visibility.Visible;
        }

        private void PlainTextSelectVisibility()
        {
            lblParentAttribute.Content = "Attribute Value";
            txtParentAttribute.Visibility = Visibility.Visible;
            ddlParentAttributes.Visibility = Visibility.Collapsed;
            ddlParentAttributes.SelectedItem = null;
            dGridNewAttributes.Visibility = Visibility.Collapsed;
            bClear.Visibility = Visibility.Collapsed;
        }

        private void GridSelectColumnsVisibility()
        {
            dGridNewAttributes.Columns[0].Visibility = Visibility.Collapsed;

            dGridNewAttributes.Columns[1].Visibility = Visibility.Visible;
            dGridNewAttributes.Columns[1].Width = new DataGridLength(150);

            dGridNewAttributes.Columns[2].Visibility = Visibility.Visible;
            dGridNewAttributes.Columns[2].Width = new DataGridLength(150);
        }

        private void GridParentDropdownSelectColumnsVisibility()
        {
            dGridNewAttributes.Columns[0].Visibility = Visibility.Visible;
            dGridNewAttributes.Columns[0].Width = new DataGridLength(300);

            dGridNewAttributes.Columns[1].Visibility = Visibility.Collapsed;
            dGridNewAttributes.Columns[2].Visibility = Visibility.Collapsed;


            // Grid need to be loaded here
            LoadAttributeValueGrid();
        }

        public void LoadAttributeValueGrid()
        {
            AttributeModel objAttributeModel = (AttributeModel)dAttributeName.SelectedItem;
            if (objAttributeModel != null)
            {
                List<AttributeValueModel> lAttributeValueModel = objDataAccess.GetAttributeValueByAttributeId(objAttributeModel.AttributeId);
                dGridNewAttributes.ItemsSource = lAttributeValueModel;
            }
        }



        private void bAddAttribute_Click(object sender, RoutedEventArgs e)
        {
            try
            {                
                int iAttributeTypeSelectedValue = Convert.ToInt32(ddlAttributeType.SelectedValue);
                int iAttributeId = 0;
                
                int iParentId = ddlParentAttributes.SelectedValue == null ? 0 : Convert.ToInt32(ddlParentAttributes.SelectedValue);                
                if (dAttributeName.SelectedValue == null)
                {
                    AttributeModel objAttributeModel = new AttributeModel()
                    {
                        AttributeName = dAttributeName.Text,
                        AttributeTypeId = iAttributeTypeSelectedValue,
                        ParentId = iParentId
                    };
                    objDataAccess.AddAttribute(objAttributeModel);
                    iAttributeId = objAttributeModel.AttributeId;
                }
                else
                {

                    iAttributeId = Convert.ToInt32(dAttributeName.SelectedValue);
                    AttributeModel objAttributeModel = new AttributeModel()
                    {
                        AttributeId = iAttributeId,
                        AttributeName = dAttributeName.Text,
                        AttributeTypeId = iAttributeTypeSelectedValue,
                        ParentId = iParentId
                    };
                    objDataAccess.ModifyAttribute(objAttributeModel);

                    AttributeModel objSelectAttributeModel = (AttributeModel)dAttributeName.SelectedItem;
                    if (objSelectAttributeModel != null && iParentId != objSelectAttributeModel.ParentId)
                    {
                        int ioldParentId = objSelectAttributeModel.ParentId;
                        List<AttributeValueModel> lOldParentsAttributeValueModel = objDataAccess.GetAttributeValueByAttributeId(ioldParentId);                        
                        // Retreiving Attribute Values based on the Parent Attribute Values
                        List<AttributeValueModel> lOldParentAttributeValueModelToBeDeleted = new List<AttributeValueModel>();
                        lOldParentsAttributeValueModel.ForEach(p => { lOldParentAttributeValueModelToBeDeleted.AddRange(objDataAccess.GetAttributeValueByParentId(p.Id)); });                        
                        // Deleting the Attribute Values which are mapped to Parent Attribute values
                        objDataAccess.DeleteAttributeValueList(lOldParentAttributeValueModelToBeDeleted);                        
                    }
                }

                if (dGridNewAttributes.Items[0].GetType().Name == nameof(NewAttributeGrid))
                {

                    List<NewAttributeGrid> lGridAttributeValues = (List<NewAttributeGrid>)dGridNewAttributes.ItemsSource;

                    List<AttributeValueModel> lNewAttributeValues = new List<AttributeValueModel>();
                    lNewAttributeValues = lGridAttributeValues.Where(p => p.AttributeId == 0).Select(p => new AttributeValueModel() { Name = p.AttributeName, AttributeId = iAttributeId, ParentValue = p.ParentAttributeValueModel.Id }).ToList();
                    objDataAccess.AddAttributeValueList(lNewAttributeValues);

                    List<AttributeValueModel> lUpdateAttributeValues = new List<AttributeValueModel>();
                    lUpdateAttributeValues = lGridAttributeValues.Where(p => p.AttributeId != 0).Select(p => new AttributeValueModel() { Id= p.AttributeId, Name = p.AttributeName, AttributeId = iAttributeId, ParentValue = p.ParentAttributeValueModel.Id }).ToList();
                    objDataAccess.UpdateAttributeValueList(lUpdateAttributeValues);

                    MessageBox.Show("Attributes Saved Successfully");
                }
                else if (dGridNewAttributes.Items[0].GetType().Name == nameof(AttributeValueModel))
                {
                    List<AttributeValueModel> lAttributeValues = (List<AttributeValueModel>)dGridNewAttributes.ItemsSource;

                    List<AttributeValueModel> lNewAttributeValues = new List<AttributeValueModel>();
                    lNewAttributeValues = lAttributeValues.Where(p => p.Id == 0 && p.AttributeId == 0).Select(p => new AttributeValueModel() { Name = p.Name, AttributeId = iAttributeId, Id = p.Id, ParentValue = p.ParentValue }).ToList();
                    objDataAccess.AddAttributeValueList(lNewAttributeValues);

                    List<AttributeValueModel> lUpdateAttributeValues = new List<AttributeValueModel>();
                    lUpdateAttributeValues = lAttributeValues.Where(p => p.Id != 0 && p.AttributeId != 0).Select(p => new AttributeValueModel() { Id = p.AttributeId,Name = p.Name, AttributeId = iAttributeId, ParentValue = p.ParentValue }).ToList();
                    objDataAccess.UpdateAttributeValueList(lUpdateAttributeValues);

                    MessageBox.Show("Attributes Saved Successfully");
                }
                ClearAttributeScreen();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Attribute Button Click; Attributes Failed to Save; " + ex.ToString());
            }


        }

        private void dAttributeName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AttributeModel objSelectedAttributeModel = (AttributeModel)dAttributeName.SelectedItem;
            if (objSelectedAttributeModel != null)
                ddlAttributeType.SelectedValue = objSelectedAttributeModel.AttributeTypeId;
            if (ddlAttributeType.SelectedItem != null)
                LoadAttributeValueGrid();

            FillParentAttributeDropdown();
            if(objSelectedAttributeModel !=null && objSelectedAttributeModel.ParentId !=0)            
                ddlParentAttributes.SelectedValue = objSelectedAttributeModel.ParentId;
            else
            {
                ddlParentAttributes.SelectedItem = null;
                if(ddlParentAttributes.SelectedValue!=null) ddlParentAttributes.SelectedValue = string.Empty;
                dGridNewAttributes.ItemsSource = new List<AttributeValueModel>();
                GridParentDropdownSelectColumnsVisibility();
            }
            
        }

        private void ddlParentAttributes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ddlParentAttributes.SelectedItem != null)
            {
                GridSelectColumnsVisibility();
                AttributeModel objAttributeModel = (AttributeModel)ddlParentAttributes.SelectedItem;
                List<AttributeValueModel> lAttributeValues = objDataAccess.GetAttributeValueByAttributeId(objAttributeModel.AttributeId);
                dGridAttributesValues.ItemsSource = lAttributeValues;
                GetAttributeValues();
            }

        }

        private void bClear_Click(object sender, RoutedEventArgs e)
        {
            ddlParentAttributes.SelectedItem = null;
            dGridNewAttributes.ItemsSource = new List<AttributeValueModel>();
            GridParentDropdownSelectColumnsVisibility();
        }

        private void GetAttributeValues()
        {
            AttributeModel objAttributeIdAttributeModel = (AttributeModel)dAttributeName.SelectedItem;
            List<AttributeValueModel> lAttributeValueModel = new List<AttributeValueModel>();
            if (objAttributeIdAttributeModel != null)
                lAttributeValueModel = objDataAccess.GetAttributeValueByAttributeId(objAttributeIdAttributeModel.AttributeId);

            AttributeModel objParentAttributeModel = (AttributeModel)ddlParentAttributes.SelectedItem;
            List<AttributeValueModel> lParentAttributeValueModel = objDataAccess.GetAttributeValueByAttributeId(objParentAttributeModel.AttributeId);

            var parentAttributeGridData = lAttributeValueModel.Join(lParentAttributeValueModel, p => p.ParentValue, q => q.Id, (p, q) => new { ParentAttributeId = q.Id, ParentAttributeName = q.Name, AttributeId = p.Id, AttributeName = p.Name });
            List<NewAttributeGrid> lNewAttributeGrid = new List<NewAttributeGrid>();
            foreach (var item in parentAttributeGridData)
            {
                AttributeValueModel objAttributeValueModel = lParentAttributeValueModel.Where(p => p.Id == item.ParentAttributeId).SingleOrDefault();
                lNewAttributeGrid.Add(new NewAttributeGrid()
                {
                    ParentAttributeValueModel = objAttributeValueModel,
                    AttributeId = item.AttributeId,
                    AttributeName = item.AttributeName
                });
            }

            dGridNewAttributes.ItemsSource = lNewAttributeGrid;
        }

        private class NewAttributeGrid
        {
            public AttributeValueModel ParentAttributeValueModel { get; set; }
            public int AttributeId { get; set; }
            public string AttributeName { get; set; }
            public string Name { get; set; }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EventSetter_OnHandler(object sender, MouseButtonEventArgs e)
        {
            object source = e.OriginalSource;
            string isDelete = ((DataGridCell)sender).Column.Header.ToString().ToUpperInvariant();
            if (source.GetType() == typeof(Image) && isDelete == "DELETE")
            {
                DataGridRow deletedSelectedRow = FindParent<DataGridRow>(sender as DependencyObject);

                if (deletedSelectedRow.DataContext.GetType().Name == nameof(NewAttributeGrid))
                {
                    // Delete the sub attribute Value
                    NewAttributeGrid objDeleteSelectedAttributeValueModel = (NewAttributeGrid)deletedSelectedRow.DataContext;
                    List<NewAttributeGrid> lNewAttributeGrid = (List<NewAttributeGrid>)dGridNewAttributes.ItemsSource;

                    if (objDeleteSelectedAttributeValueModel.AttributeId > 0)
                    {
                        AttributeValueModel objAttributeValueModel = new AttributeValueModel() { Id = objDeleteSelectedAttributeValueModel.AttributeId, Name = objDeleteSelectedAttributeValueModel.AttributeName };
                        bool isResult = objDataAccess.DeleteAttributeValue(objAttributeValueModel);
                        if (isResult)
                        {
                            lNewAttributeGrid.Remove(objDeleteSelectedAttributeValueModel);
                            dGridNewAttributes.ItemsSource = null;
                            dGridNewAttributes.ItemsSource = lNewAttributeGrid;
                            MessageBox.Show(objDeleteSelectedAttributeValueModel.AttributeName + " Deleted Successfully");
                        }
                    }
                }
                else if (deletedSelectedRow.DataContext.GetType().Name == nameof(AttributeValueModel))
                {
                    // Delete the attribute Value
                    AttributeValueModel objDeleteSelectedAttributeValueModel = (AttributeValueModel)deletedSelectedRow.DataContext;
                    List<AttributeValueModel> lAttributeValueModel = (List<AttributeValueModel>)dGridNewAttributes.ItemsSource;
                    if (objDeleteSelectedAttributeValueModel.Id > 0){
                        bool isResult = objDataAccess.DeleteAttributeValue(objDeleteSelectedAttributeValueModel);
                        if (isResult)
                        {
                            lAttributeValueModel.Remove(objDeleteSelectedAttributeValueModel);
                            dGridNewAttributes.ItemsSource = null;
                            dGridNewAttributes.ItemsSource = lAttributeValueModel;
                            MessageBox.Show(objDeleteSelectedAttributeValueModel.Name + " Deleted Successfully");
                        }
                    }
                }
            }
        }

        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;
            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindParent<T>(parentObject);
        }

        private void bEnterNewAttribute_Click(object sender, RoutedEventArgs e)
        {
            dAttributeName.ItemsSource = null;
            ddlAttributeType.ItemsSource = null;
            dGridNewAttributes.ItemsSource = null;
            LoadAttributes();
            LoadAttributeTypes();
        }

        private void bDeleteAttribute_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure to delete Attribute and Attribute Values?", "Delete Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                int iAttributeId = Convert.ToInt32(dAttributeName.SelectedValue);
                List<AttributeValueModel> lAttributeValueList = objDataAccess.GetAttributeValueByAttributeId(iAttributeId);
                bool isAttributeValueResult = objDataAccess.DeleteAttributeValueList(lAttributeValueList);

                List<AttributeValueModel> lAttributeValuesParentToBeNull = new List<AttributeValueModel>();
                List<int> lParentsIds = lAttributeValueList.Select(p => p.ParentValue).Distinct().ToList();
                foreach (int iParentId in lParentsIds)
                {
                    lAttributeValuesParentToBeNull.AddRange(objDataAccess.GetAttributeValueByParentId(iParentId));
                }
                lAttributeValuesParentToBeNull.ForEach(p => { p.ParentValue = 0; });
                bool isParentValuesUpdated = objDataAccess.UpdateAttributeValueList(lAttributeValuesParentToBeNull);

                bool isAttributeResult = true;
                if (isAttributeValueResult && isParentValuesUpdated)
                    isAttributeResult = objDataAccess.DeleteAttribute(iAttributeId);

                if (isAttributeResult)
                {
                    MessageBox.Show("Attribute and Attribute Values deleted successfully");
                    ClearAttributeScreen();
                }
            }
        }

        public void ClearAttributeScreen()
        {
            LoadAttributes();
            LoadAttributeTypes();
            dGridNewAttributes.ItemsSource = null;
            dAttributeName.Text = string.Empty;
            ddlParentAttributes.Visibility = Visibility.Collapsed;
            dGridNewAttributes.Visibility = Visibility.Collapsed;
            bClear.Visibility = Visibility.Collapsed;
        }
    }
}
