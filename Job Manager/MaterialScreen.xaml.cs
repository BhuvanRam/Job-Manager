using JobManager.DAL;
using JobManager.Model;
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
    /// Interaction logic for MaterialScreen.xaml
    /// </summary>
    public partial class MaterialScreen : Window
    {
        DataAccess objDataAccess = new DataAccess();
        public List<MaterialGridAttribute> lAttributeModels { get; set; }
        public List<MaterialGridAttribute> lSelectedModels { get; set; }

      
       
        public MaterialScreen()
        {
            InitializeComponent();
            LoadMaterials();
            LoadMaterialTypes();
            LoadAttributes();
        }
        
     

        public void LoadMaterials()
        {
            List<Material> lMaterials = objDataAccess.GetMaterials();
            dMaterials.ItemsSource = lMaterials;
        }

        public void LoadMaterialTypes()
        {
            List<MaterialType> lMaterialTypes = objDataAccess.GetMaterialTypes();
            dMaterialType.ItemsSource = lMaterialTypes;
        }

        public void LoadAttributes()
        {
            List<AttributeModel> lAttributes = objDataAccess.GetAllAttributes();
            List<MaterialGridAttribute> lMaterialGridAttribute = new List<MaterialGridAttribute>();
            lAttributes.ForEach(p => {
                MaterialGridAttribute objMaterialGridAttribute = new MaterialGridAttribute()
                {
                    Attribute = p,
                    IsSelected=false                    
                };
                lMaterialGridAttribute.Add(objMaterialGridAttribute);
            });
            dAllAttributes.ItemsSource = lMaterialGridAttribute;
        }


        private void dSelectedAttributesEventSetter_OnHandlerDelete(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure to delete Attribute for the selected Material", "Delete Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                object source = e.OriginalSource;
                string isDelete = ((DataGridCell)sender).Column.Header.ToString().ToUpperInvariant();
                if (source.GetType() == typeof(Image) && isDelete == "DELETE")
                {
                    DataGridRow deletedSelectedRow = FindParent<DataGridRow>(sender as DependencyObject);
                    MaterialGridAttribute objSelectedMaterialGridAttribute = (MaterialGridAttribute)deletedSelectedRow.DataContext;
                    lSelectedModels.Remove(objSelectedMaterialGridAttribute);
                    int iMaterialId = Convert.ToInt32(dMaterials.SelectedValue);
                    objDataAccess.DeleteAttributeFromMaterial(iMaterialId, objSelectedMaterialGridAttribute.Attribute.AttributeId);
                    objSelectedMaterialGridAttribute.IsSelected = false;
                    lAttributeModels.Add(objSelectedMaterialGridAttribute);
                    dAllAttributes.ItemsSource = null;
                    dSelectedAttributes.ItemsSource = null;
                    dAllAttributes.ItemsSource = lAttributeModels;
                    dSelectedAttributes.ItemsSource = lSelectedModels;
                    MessageBox.Show("Attribute for the selected Material deleted successfully");
                    // Reload Grid
                }
            }              
        }

      

        private void bAddAttributesToBelowGridToSave_Click(object sender, RoutedEventArgs e)
        {
            lAttributeModels = (List<MaterialGridAttribute>)dAllAttributes.ItemsSource;
            if (lSelectedModels == null)
                lSelectedModels = new List<MaterialGridAttribute>();
            lSelectedModels.AddRange(lAttributeModels.Where(p => p.IsSelected == true).ToList());
            dSelectedAttributes.ItemsSource = null;
            dSelectedAttributes.ItemsSource = lSelectedModels;
            lAttributeModels = lAttributeModels.Where(p => p.IsSelected == false).ToList();
            dAllAttributes.ItemsSource = lAttributeModels;            
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

        private void dMaterials_Selected(object sender, RoutedEventArgs e)
        {
            int iMaterialId = Convert.ToInt32(dMaterials.SelectedValue);
            List<MaterialAttribute> lSelectedMaterialAttribute =new List<MaterialAttribute>();
            if (iMaterialId != 0)
            {

                Material objSelectedMaterial = (Material)dMaterials.SelectedItem;
                dMaterialType.SelectedValue = objSelectedMaterial.MaterialType.Id;
                cIsEnable.IsChecked = objSelectedMaterial.IsActive;


                lSelectedMaterialAttribute = objDataAccess.GetMaterialAttributesByMaterialId(objSelectedMaterial.Id);
                if (lSelectedModels == null)
                    lSelectedModels = new List<MaterialGridAttribute>();


                lSelectedModels.Clear();
                lSelectedMaterialAttribute.ForEach(p =>
                {
                    lSelectedModels.Add(new MaterialGridAttribute()
                    {
                        Attribute = new AttributeModel()
                        {
                            AttributeId = p.Attribute.Id,
                            AttributeName = p.Attribute.Name,
                            AttributeTypeId = p.Attribute.TypeId,
                            ParentId = p.Attribute.ParentId == null ? 0 : (int)p.Attribute.ParentId
                        },
                        IsSelected = true
                    });
                });
                dSelectedAttributes.ItemsSource = null;
                dSelectedAttributes.ItemsSource = lSelectedModels;
            }
            else
            {
                lSelectedModels = new List<MaterialGridAttribute>();
                dSelectedAttributes.ItemsSource = null;
            }
            List<AttributeModel> lAllAttributes = objDataAccess.GetAllAttributes();

                if (lAttributeModels == null)
                    lAttributeModels = new List<MaterialGridAttribute>();
                lAttributeModels.Clear();
                int[] distinctSelectedAttributeId = lSelectedMaterialAttribute?.Select(p => p.AttributeId).ToArray();
                var AllAttributes = lAllAttributes.Where(p => !distinctSelectedAttributeId.Contains(p.AttributeId));

                AllAttributes.ToList().ForEach(p => {                  
                        MaterialGridAttribute objMaterialGridAttribute = new MaterialGridAttribute()
                        {
                            Attribute = p,
                            IsSelected = false
                        };
                        lAttributeModels.Add(objMaterialGridAttribute);
                    
                });
                dAllAttributes.ItemsSource = null;
                dAllAttributes.ItemsSource = lAttributeModels;
            
            
        }

        private void bCleart_Click(object sender, RoutedEventArgs e)
        {
            ClearScreen();
        }

        public void ClearScreen()
        {
            dMaterials.Text = string.Empty;
            dMaterials.SelectedItem = null;
            dMaterialType.SelectedItem = null;
            cIsEnable.IsChecked = false;
            dSelectedAttributes.ItemsSource = null;
            dAllAttributes.ItemsSource = null;
            if (lAttributeModels != null)
                lAttributeModels.Clear();
            if (lSelectedModels != null)
                lSelectedModels.Clear();
            LoadMaterials();
            LoadMaterialTypes();
            LoadAttributes();
        }

        private void bSave_Click(object sender, RoutedEventArgs e)
        {
            int iMaterialId = Convert.ToInt32(dMaterials.SelectedValue);

            if(iMaterialId != 0)
            {
                Material objSelectedMaterial = new Material();
                objSelectedMaterial.Id = Convert.ToInt32(dMaterials.SelectedValue);
                objSelectedMaterial.Name = dMaterials.Text;
                objSelectedMaterial.TypeId = Convert.ToInt32(dMaterialType.SelectedValue); 
                objSelectedMaterial.IsActive= Convert.ToBoolean(cIsEnable.IsChecked);

                List<MaterialGridAttribute> objSelectedMaterialGridAttribute = (List<MaterialGridAttribute>)dSelectedAttributes.ItemsSource;
                List<MaterialAttribute> lSelectedMaterialAttributes = new List<MaterialAttribute>();
                for (int i=1;i<= objSelectedMaterialGridAttribute.Count();i++)
                {
                    MaterialAttribute objMaterialAttribute = new MaterialAttribute();
                    objMaterialAttribute.MaterialId = objSelectedMaterial.Id;
                    objMaterialAttribute.AttributeId = objSelectedMaterialGridAttribute[i-1].Attribute.AttributeId;                    
                    objMaterialAttribute.SortOder = i;
                    objMaterialAttribute.IsRequired = true;
                    lSelectedMaterialAttributes.Add(objMaterialAttribute);
                }

                bool isMaterialUpdate = objDataAccess.UpdateMaterial(objSelectedMaterial);
                if (isMaterialUpdate)
                {
                    bool isMaterialAttributeUpdate = objDataAccess.UpdateMaterialAttributes(lSelectedMaterialAttributes);
                    if (isMaterialAttributeUpdate)
                        MessageBox.Show("Material Saved successfully");
                }
                else
                    MessageBox.Show("Material Failed to Save");
                // For existing material save
            }
            else
            {
                // For New Material save
                Material objNewMaterial = new Material();
                objNewMaterial.Name = dMaterials.Text;
                objNewMaterial.IsActive = Convert.ToBoolean(cIsEnable.IsChecked);
                objNewMaterial.TypeId = Convert.ToInt32(dMaterialType.SelectedValue);
                bool isSuccesss = objDataAccess.AddMaterial(objNewMaterial);

                List<MaterialGridAttribute> objSelectedMaterialGridAttribute = (List<MaterialGridAttribute>)dSelectedAttributes.ItemsSource;
                if (objSelectedMaterialGridAttribute != null)
                {
                    List<MaterialAttribute> lSelectedMaterialAttributes = new List<MaterialAttribute>();
                    for (int i = 1; i <= objSelectedMaterialGridAttribute.Count(); i++)
                    {
                        MaterialAttribute objMaterialAttribute = new MaterialAttribute();
                        objMaterialAttribute.MaterialId = objNewMaterial.Id;
                        objMaterialAttribute.AttributeId = objSelectedMaterialGridAttribute[i - 1].Attribute.AttributeId;
                        objMaterialAttribute.SortOder = i;
                        objMaterialAttribute.IsRequired = true;
                        lSelectedMaterialAttributes.Add(objMaterialAttribute);
                    }
                    if (isSuccesss)
                    {
                        bool isMaterialAttributeUpdate = objDataAccess.UpdateMaterialAttributes(lSelectedMaterialAttributes);
                        if (isMaterialAttributeUpdate)
                            MessageBox.Show("Material Saved successfully");
                    }
                    else
                        MessageBox.Show("Material Failed to Save");
                }
                else
                {
                    MessageBox.Show("Material cannot be saved without selecting selected attribute");
                }
                
            }
            ClearScreen();
        }

        private void bDelete_Click(object sender, RoutedEventArgs e)
        {
            int iMaterialId = Convert.ToInt32(dMaterials.SelectedValue);
            if (iMaterialId != 0)
            {
                bool isDelete = objDataAccess.DeleteMaterial(iMaterialId);
                MessageBox.Show("Material deleted successfully");
            }
            else
            {
                MessageBox.Show("Select any Material to Delete");
            }
            ClearScreen();
        }

        private void bReOrderUpButton_Click(object sender, RoutedEventArgs e)
        {
            MaterialGridAttribute objSelectedAttribute = (MaterialGridAttribute)dSelectedAttributes.SelectedValue;
            int selectedIndex = dSelectedAttributes.SelectedIndex;
            if (selectedIndex <= 0)
                return;

            List<MaterialGridAttribute> selectedAttributes = (List<MaterialGridAttribute>)dSelectedAttributes.ItemsSource;
            selectedAttributes.RemoveAt(selectedIndex);
            selectedAttributes.Insert(selectedIndex - 1, objSelectedAttribute);
            dSelectedAttributes.ItemsSource = null;
            dSelectedAttributes.ItemsSource = selectedAttributes;
            dSelectedAttributes.SelectedValue = objSelectedAttribute;

        }

        private void bReOrderDownButton_Click(object sender, RoutedEventArgs e)
        {
            MaterialGridAttribute objSelectedAttribute = (MaterialGridAttribute)dSelectedAttributes.SelectedValue;
            int selectedIndex = dSelectedAttributes.SelectedIndex;
            if (selectedIndex >= dSelectedAttributes.Items.Count-1)
                return;

            List<MaterialGridAttribute> selectedAttributes = (List<MaterialGridAttribute>)dSelectedAttributes.ItemsSource;
            selectedAttributes.RemoveAt(selectedIndex);
            selectedAttributes.Insert(selectedIndex + 1, objSelectedAttribute);
            dSelectedAttributes.ItemsSource = null;
            dSelectedAttributes.ItemsSource = selectedAttributes;
            dSelectedAttributes.SelectedValue = objSelectedAttribute;
        }
    }

    public class MaterialGridAttribute
    {
        public bool IsSelected { get; set; }
        public AttributeModel Attribute { get; set; }
    }

    
}
