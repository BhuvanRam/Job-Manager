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
using System.Data;

namespace Job_Manager
{
    /// <summary>
    /// Interaction logic for AddMaterialToJob.xaml
    /// </summary>
    public partial class AddMaterialToJob : Window
    {
        DataAccess da = new DataAccess();
        DataTable dtMaterialAttributes = new DataTable();
        int jobId = 1;
        public AddMaterialToJob(int jobIdParam)
        {
            InitializeComponent();
            jobId = jobIdParam;
            dtMaterialAttributes.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("JobId",typeof(int)),
                new DataColumn("MaterialId",typeof(int)),
                new DataColumn("AttributeId",typeof(int)),
                new DataColumn("TypeId",typeof(int)),
                new DataColumn("ControlName",typeof(string)),
                new DataColumn("ValueId",typeof(int)),
                new DataColumn("Value",typeof(string))
            });

            LoadMaterialCombo();
        }
        public AddMaterialToJob(JobMaterialModel jm)
        {
            InitializeComponent();

            dtMaterialAttributes.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("JobId",typeof(int)),
                new DataColumn("MaterialId",typeof(int)),
                new DataColumn("AttributeId",typeof(int)),
                new DataColumn("TypeId",typeof(int)),
                new DataColumn("ControlName",typeof(string)),
                new DataColumn("ValueId",typeof(int)),
                new DataColumn("Value",typeof(string))
            });

            LoadMaterialCombo();
            
        }
        private void LoadMaterialCombo()
        {
            List<MaterialModel> materials = da.GetMaterialsForJob(jobId);

            cmbMaterial.DisplayMemberPath = "Name";
            cmbMaterial.SelectedValuePath = "Id";
            cmbMaterial.ItemsSource = materials;
            cmbMaterial.SelectedValue = -1;
        }
       
        private void cmbMaterial_Selected(object sender, SelectionChangedEventArgs e)
        {
            dtMaterialAttributes.Rows.Clear();
            List<MaterialAttributeModel> attributes = da.GetAttributesOfMaterial(Convert.ToInt32(cmbMaterial.SelectedValue));
            double defaultTop = 40;
            this.dynamicsCanvas.Children.Clear();
            foreach (MaterialAttributeModel ma in attributes)
            {
                DataRow dr = dtMaterialAttributes.NewRow();
                dr["JobId"] = jobId;
                dr["MaterialId"] = Convert.ToInt32(cmbMaterial.SelectedValue);
                dr["AttributeId"] = ma.Id;
                dr["TypeId"] = ma.TypeId;
                

                Label lblAttribute = new Label();
                lblAttribute.Content = ma.Name;
                lblAttribute.Width = 100;
                this.dynamicsCanvas.Children.Add(lblAttribute);
                Canvas.SetLeft(lblAttribute, 10);
                Canvas.SetTop(lblAttribute, defaultTop);
                if (ma.TypeId == 1)
                {
                    TextBox txtAttribute = new TextBox();
                    txtAttribute.Width = 150;
                    txtAttribute.Name = "cmbAttribute_" + ma.Id.ToString() + "_0";
                    dr["ControlName"] = txtAttribute.Name;
                    this.dynamicsCanvas.Children.Add(txtAttribute);
                    Canvas.SetLeft(txtAttribute, 125);
                    Canvas.SetTop(txtAttribute, defaultTop);
                }
                else
                {
                    ComboBox cmbAttribute = new ComboBox();
                    cmbAttribute.Width = 150;
                    if(ma.ParentId.HasValue)
                      cmbAttribute.Name = "cmbAttribute_" + ma.Id.ToString() +"_"+ma.ParentId.ToString();
                    else
                      cmbAttribute.Name = "cmbAttribute_" + ma.Id.ToString() + "_0";
                    dr["ControlName"] = cmbAttribute.Name;
                    this.dynamicsCanvas.Children.Add(cmbAttribute);
                    Canvas.SetLeft(cmbAttribute, 125);
                    Canvas.SetTop(cmbAttribute, defaultTop);
                    if (!ma.ParentId.HasValue)
                    {
                        List<AttributeValueModel> values = da.GetAttributeValues(ma.Id);
                        cmbAttribute.DisplayMemberPath = "Name";
                        cmbAttribute.SelectedValuePath = "Id";
                        cmbAttribute.ItemsSource = values;
                        cmbAttribute.SelectedValue = -1;
                      
                    }
                    if (ma.HasChilds.HasValue)
                    {
                        cmbAttribute.SelectionChanged += CmbAttribute_SelectionChanged;
                    }
                }
                defaultTop = defaultTop + 30;
                dtMaterialAttributes.Rows.Add(dr);
            }
        }

        private void CmbAttribute_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IEnumerator<ComboBox> controls = this.dynamicsCanvas.Children.OfType<ComboBox>().GetEnumerator();
            controls.MoveNext();
            while (controls.Current !=null)
            {
                if(((ComboBox)controls.Current).Name.Split('_')[2]  == ((ComboBox)sender).Name.Split('_')[1])
                {
                    ComboBox comboChild = (ComboBox)controls.Current;
                    List<AttributeValueModel> values = da.GetChildAttributeValues(Convert.ToInt32(((ComboBox)sender).SelectedValue));
                    comboChild.DisplayMemberPath = "Name";
                    comboChild.SelectedValuePath = "Id";
                    comboChild.ItemsSource = values;
                    comboChild.SelectedValue = -1;
                    break;
                }
                controls.MoveNext();    
            }

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try { 
            foreach(DataRow dr in dtMaterialAttributes.Rows)
            {
                if (Convert.ToInt32(dr["TypeId"]) == 2)
                {
                    IEnumerator<ComboBox> controls = this.dynamicsCanvas.Children.OfType<ComboBox>().GetEnumerator();
                    controls.MoveNext();
                    while (controls.Current != null)
                    {
                        if(controls.Current.Name == dr["ControlName"].ToString())
                        {
                            ComboBox comboChild = (ComboBox)controls.Current;
                            dr["ValueId"] = Convert.ToInt32(comboChild.SelectedValue);
                            dr["value"] = string.Empty;
                            break;
                        }
                        controls.MoveNext();
                    }
                    
                }
                else
                {
                    IEnumerator<TextBox> controls = this.dynamicsCanvas.Children.OfType<TextBox>().GetEnumerator();
                    controls.MoveNext();
                    while (controls.Current != null)
                    {
                        if (controls.Current.Name == dr["ControlName"].ToString())
                        {
                            TextBox txtChild = (TextBox)controls.Current;
                            dr["Value"] = txtChild.Text;
                            dr["ValueId"] = 0;
                            break;
                        }
                        controls.MoveNext();
                    }
                }
            }
            da.InsertUpdateJobMaterial(dtMaterialAttributes);

            MessageBox.Show("Selected Material Successfully added to the Job");
            LoadMaterialCombo();

            var myObject = this.Owner as JobDetails;
            myObject.LoadJobDetails();
            }catch(Exception ex)
            {
                if (ex.InnerException != null)
                    MessageBox.Show("Message:"+ ex.Message + "Inner Message:" + ex.InnerException.Message);
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();           
        }
    }
}
