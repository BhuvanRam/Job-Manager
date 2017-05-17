using JobManager.DAL;
using JobManager.Model;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Job_Manager
{
    /// <summary>
    /// Interaction logic for ReportViewer.xaml
    /// </summary>
    public partial class ReportViewer : UserControl
    {

        public static readonly DependencyProperty _dReportId= DependencyProperty.Register
         (
              "ReportId",
              typeof(string),
              typeof(ReportViewer),
              new PropertyMetadata(string.Empty)
         );

        public string ReportId
        {
            get { return (string)GetValue(_dReportId); }
            set { SetValue(_dReportId, value); }
        }

        DataAccess objDataAccess = new DataAccess();
        List<MasterData> lMasterData = new List<MasterData>();
        public ReportViewer()
        {
            
            InitializeComponent();
            lMasterData = objDataAccess.GetMasterData("Global");
        }

        decimal iTotalAmount = 0;
        
        

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DataTable dtCompanyDetails = new DataTable(); 

            PurchaseOrder objPurchaseOrder = objDataAccess.GetPurchaseOrderById(Convert.ToInt32(ReportId));
            List<JobPOMaterialModel> lJobMaterials = objDataAccess.GetJobPOMaterials(objPurchaseOrder.JobId, objPurchaseOrder.Id);
            JobPOModel objJobPOModel =  objDataAccess.GetJobPODetails(objPurchaseOrder.Id);
            JobModel objJobDetails = objDataAccess.GetJobDetails(objPurchaseOrder.JobId);
           
            if (objJobDetails.BranchId!=null)
                dtCompanyDetails = CompanyDetails(Convert.ToInt32(objJobDetails.BranchId));

            DataTable dtToDetails = GetVendorDetails(objPurchaseOrder);
            DataTable dtReportContents = ReportContents(lJobMaterials);

            decimal? dTotalValue;
            DataTable dtTermsConditions = Terms_ConditionsContent(objJobPOModel,out dTotalValue);
            DataTable dtTotal = new DataTable();
            dtTotal.Columns.Add("TotalValue", typeof(decimal));

            DataRow drTotalRow = dtTotal.NewRow();
            drTotalRow["TotalValue"] = Convert.ToDecimal(dTotalValue);
            dtTotal.Rows.Add(drTotalRow);

            ReportDataSource rhCompanyDetailsDataSource = new ReportDataSource("rhCompanyDetails", dtCompanyDetails);
            ReportDataSource rhToDetailsDataSource = new ReportDataSource("rhToDetail", dtToDetails);
            ReportDataSource rhReportContentsDataSource = new ReportDataSource("rhContents", dtReportContents);
            ReportDataSource rhReportTermsConditions = new ReportDataSource("rhTerms_Conditions", dtTermsConditions);
            ReportDataSource rh_Total = new ReportDataSource("rh_Total", dtTotal);

            reportViewer.LocalReport.ReportPath = "PurchaseOrderReport.rdlc";                        
            reportViewer.LocalReport.DataSources.Add(rhCompanyDetailsDataSource);
            reportViewer.LocalReport.DataSources.Add(rhToDetailsDataSource);
            reportViewer.LocalReport.DataSources.Add(rhReportContentsDataSource);
            reportViewer.LocalReport.DataSources.Add(rhReportTermsConditions);
            reportViewer.LocalReport.DataSources.Add(rh_Total);
            reportViewer.RefreshReport();
        }

        private DataTable Terms_ConditionsContent(JobPOModel objJobPOModel,out decimal? iTotal)
        {

            DataTable dtTCContents = new DataTable();
            dtTCContents.Columns.Add(new DataColumn("Terms_Conditions", typeof(string)));
            dtTCContents.Columns.Add(new DataColumn("Details", typeof(decimal)));
            dtTCContents.Columns.Add(new DataColumn("Amount", typeof(decimal)));

            iTotal = iTotalAmount;

            DataRow drNewRow = dtTCContents.NewRow();
            drNewRow["Terms_Conditions"] = "Discount";
            drNewRow["Details"] = objJobPOModel.Discount;         
            drNewRow["Amount"] = objJobPOModel.Discount;
            iTotal = iTotal - objJobPOModel.Discount;
            dtTCContents.Rows.Add(drNewRow);

            
            drNewRow = dtTCContents.NewRow();
            drNewRow["Terms_Conditions"] = "Advance Payment";
            drNewRow["Details"] = Convert.ToDecimal(objJobPOModel.Payment);
            drNewRow["Amount"] = Convert.ToDecimal(objJobPOModel.Payment);
            iTotal = iTotal - objJobPOModel.Payment;
            dtTCContents.Rows.Add(drNewRow);

            drNewRow = dtTCContents.NewRow();
            drNewRow["Terms_Conditions"] = "Delivery";
            drNewRow["Details"] = Convert.ToDecimal(objJobPOModel.Delivery);
            drNewRow["Amount"] = Convert.ToDecimal(objJobPOModel.Delivery);
            iTotal = iTotal + Convert.ToDecimal(objJobPOModel.Delivery);
            dtTCContents.Rows.Add(drNewRow);

            decimal dPacking = Convert.ToDecimal(objJobPOModel.Packing);
            drNewRow = dtTCContents.NewRow();
            drNewRow["Terms_Conditions"] = "Packing";
            drNewRow["Details"] = dPacking;
            drNewRow["Amount"] = (dPacking * iTotalAmount) / 100;
            iTotal = iTotal + ((dPacking * iTotalAmount) / 100);
            dtTCContents.Rows.Add(drNewRow);

            decimal dExciseDuty = Convert.ToDecimal(objJobPOModel.ExciseDuty);
            drNewRow = dtTCContents.NewRow();
            drNewRow["Terms_Conditions"] = "Excise Duty";
            drNewRow["Details"] = dExciseDuty;
            drNewRow["Amount"] = (dExciseDuty * iTotalAmount) / 100;
            iTotal = iTotal + ((dExciseDuty * iTotalAmount) / 100);
            dtTCContents.Rows.Add(drNewRow);


            decimal dTransportInsurance = Convert.ToDecimal(objJobPOModel.TransportInsurance);
            drNewRow = dtTCContents.NewRow();
            drNewRow["Terms_Conditions"] = "Transport Insurance";
            drNewRow["Details"] = dTransportInsurance;
            drNewRow["Amount"] = (dTransportInsurance * iTotalAmount) / 100;
            iTotal = iTotal + ((dTransportInsurance * iTotalAmount) / 100);
            dtTCContents.Rows.Add(drNewRow);

            decimal dTransportation = Convert.ToDecimal(objJobPOModel.TransportInsurance);
            drNewRow = dtTCContents.NewRow();
            drNewRow["Terms_Conditions"] = "Transportation";
            drNewRow["Details"] = dTransportation;
            drNewRow["Amount"] = (dTransportation * iTotalAmount) / 100;
            iTotal = iTotal + ((dTransportation * iTotalAmount) / 100);
            dtTCContents.Rows.Add(drNewRow);

            decimal dOctroi = Convert.ToDecimal(objJobPOModel.TransportInsurance);
            drNewRow = dtTCContents.NewRow();
            drNewRow["Terms_Conditions"] = "Octroi";
            drNewRow["Details"] = dOctroi;
            drNewRow["Amount"] = (dOctroi * iTotalAmount) / 100;
            iTotal = iTotal + ((dOctroi * iTotalAmount) / 100);
            dtTCContents.Rows.Add(drNewRow);

            decimal taxes  = Convert.ToDecimal(objJobPOModel.TaxesExtra);
            drNewRow = dtTCContents.NewRow();            
            drNewRow["Terms_Conditions"] = "Service Tax";
            drNewRow["Details"] = taxes;
            drNewRow["Amount"] = (taxes * iTotalAmount) / 100;
            iTotal = iTotal + ((taxes * iTotalAmount) / 100);
            dtTCContents.Rows.Add(drNewRow);

            drNewRow = dtTCContents.NewRow();
            drNewRow["Terms_Conditions"] = "Round Off";
            drNewRow["Details"] = 0.00;
            drNewRow["Amount"] = 0.00;
            dtTCContents.Rows.Add(drNewRow);

            return dtTCContents;

        }

        private DataTable ReportContents(List<JobPOMaterialModel> lJobPOMaterials)
        {
            DataTable dtReportContents = new DataTable();
            dtReportContents.Columns.Add(new DataColumn("SNO", typeof(int)));
            dtReportContents.Columns.Add(new DataColumn("Particulars", typeof(string)));
            dtReportContents.Columns.Add(new DataColumn("Quantity", typeof(int)));
            dtReportContents.Columns.Add(new DataColumn("Units", typeof(int)));
            dtReportContents.Columns.Add(new DataColumn("UnitRate", typeof(int)));
            dtReportContents.Columns.Add(new DataColumn("Amount", typeof(int)));

            int i = 0;
            foreach (JobPOMaterialModel itemJobPOMaterial in lJobPOMaterials)
            {
                DataRow drNewRow = dtReportContents.NewRow();
                drNewRow["SNO"] = i + 1; ;
                drNewRow["Particulars"] = itemJobPOMaterial.Name;
                drNewRow["Quantity"] = itemJobPOMaterial.Quantity;
                drNewRow["Units"] = 1;
                drNewRow["UnitRate"] = itemJobPOMaterial.Price;
                drNewRow["Amount"] = itemJobPOMaterial.Quantity * itemJobPOMaterial.Price;
                dtReportContents.Rows.Add(drNewRow);

                iTotalAmount = iTotalAmount + (itemJobPOMaterial.Quantity * itemJobPOMaterial.Price);
            }
            return dtReportContents;
        }

        private DataTable GetVendorDetails(PurchaseOrder objPurchaseOrder)
        {
            Vendor objVendor = objPurchaseOrder.Vendor;
            DataTable dtToDetails = new DataTable();

            dtToDetails.Columns.Add(new DataColumn("ColumnName", typeof(string)));
            dtToDetails.Columns.Add(new DataColumn("ColumnData", typeof(string)));

            DataRow drNewRow = dtToDetails.NewRow();
            drNewRow["ColumnName"] = "To";
            drNewRow["ColumnData"] = "";
            dtToDetails.Rows.Add(drNewRow);

            drNewRow = dtToDetails.NewRow();
            drNewRow["ColumnName"] = "M/S.";
            drNewRow["ColumnData"] = objVendor.VendorName;
            dtToDetails.Rows.Add(drNewRow);            

            drNewRow = dtToDetails.NewRow();
            drNewRow["ColumnName"] = "Address 1";
            drNewRow["ColumnData"] = objVendor.Address1;
            dtToDetails.Rows.Add(drNewRow);

            drNewRow = dtToDetails.NewRow();
            drNewRow["ColumnName"] = "Address 2";
            drNewRow["ColumnData"] = objVendor.Address2;
            dtToDetails.Rows.Add(drNewRow);

            drNewRow = dtToDetails.NewRow();
            drNewRow["ColumnName"] = "Address 3";
            drNewRow["ColumnData"] = "State : " + objVendor.State.StateName + " , City : " + objVendor.City;
            dtToDetails.Rows.Add(drNewRow);

            drNewRow = dtToDetails.NewRow();
            drNewRow["ColumnName"] = "Phone Number";
            drNewRow["ColumnData"] = objVendor.PhoneNo;
            dtToDetails.Rows.Add(drNewRow);

            drNewRow = dtToDetails.NewRow();
            drNewRow["ColumnName"] = "Fax Number";
            drNewRow["ColumnData"] = objVendor.FaxNo;
            dtToDetails.Rows.Add(drNewRow);

            drNewRow = dtToDetails.NewRow();
            drNewRow["ColumnName"] = "PO. No: GT";
            drNewRow["ColumnData"] = "";
            dtToDetails.Rows.Add(drNewRow);

            drNewRow = dtToDetails.NewRow();
            drNewRow["ColumnName"] = "Date";
            drNewRow["ColumnData"] = DateTime.Now.ToString();
            dtToDetails.Rows.Add(drNewRow);

            drNewRow = dtToDetails.NewRow();
            drNewRow["ColumnName"] = "RefNo&DT";
            drNewRow["ColumnData"] = objPurchaseOrder.ApprovedOn;
            dtToDetails.Rows.Add(drNewRow);

            drNewRow = dtToDetails.NewRow();
            drNewRow["ColumnName"] = "Kind Attn";
            drNewRow["ColumnData"] = objVendor.ContactPerson;
            dtToDetails.Rows.Add(drNewRow);

            return dtToDetails;

        }

        private DataTable CompanyDetails(int iBranchId)
        {            
            Branch branchDetail = objDataAccess.GetBranchDetails(iBranchId);
        
            DataTable dtCompanyDetails = new DataTable();

            dtCompanyDetails.Columns.Add(new DataColumn("ColumnName", typeof(string)));
            dtCompanyDetails.Columns.Add(new DataColumn("ColumnData", typeof(string)));         

            DataRow drNewRow = dtCompanyDetails.NewRow();
            drNewRow["ColumnName"] = "Company Name";
            drNewRow["ColumnData"] = branchDetail.Name;
            dtCompanyDetails.Rows.Add(drNewRow);

            drNewRow = dtCompanyDetails.NewRow();
            drNewRow["ColumnName"] = "Company Address";
            drNewRow["ColumnData"] = branchDetail.Address1 + "," + branchDetail.Address2 + "," + branchDetail.City + "," + branchDetail.State;//GetMasterDataBasedOnValueName("Company Address");
            dtCompanyDetails.Rows.Add(drNewRow);

            drNewRow = dtCompanyDetails.NewRow();
            drNewRow["ColumnName"] = "Phone No";
            drNewRow["ColumnData"] = branchDetail.Phone;
            dtCompanyDetails.Rows.Add(drNewRow);

            drNewRow = dtCompanyDetails.NewRow();
            drNewRow["ColumnName"] = "Fax No";
            drNewRow["ColumnData"] = branchDetail.Fax;
            dtCompanyDetails.Rows.Add(drNewRow);

            drNewRow = dtCompanyDetails.NewRow();
            drNewRow["ColumnName"] = "Email";
            drNewRow["ColumnData"] = branchDetail.Email;
            dtCompanyDetails.Rows.Add(drNewRow);

            drNewRow = dtCompanyDetails.NewRow();
            drNewRow["ColumnName"] = "TinNo";
            drNewRow["ColumnData"] = branchDetail.TIN;
            dtCompanyDetails.Rows.Add(drNewRow);

            drNewRow = dtCompanyDetails.NewRow();
            drNewRow["ColumnName"] = "CstNo";
            drNewRow["ColumnData"] =  GetMasterDataBasedOnValueName("CstNo");
            dtCompanyDetails.Rows.Add(drNewRow);

            drNewRow = dtCompanyDetails.NewRow();
            drNewRow["ColumnName"] = "EccNo";
            drNewRow["ColumnData"] = branchDetail.ECC;
            dtCompanyDetails.Rows.Add(drNewRow);

            drNewRow = dtCompanyDetails.NewRow();
            drNewRow["ColumnName"] = "RangeDivision";
            drNewRow["ColumnData"] = branchDetail.Range;
            dtCompanyDetails.Rows.Add(drNewRow);

            drNewRow = dtCompanyDetails.NewRow();
            drNewRow["ColumnName"] = "BuyerName";
            drNewRow["ColumnData"] = GetMasterDataBasedOnValueName("BuyerName");
            dtCompanyDetails.Rows.Add(drNewRow);

            drNewRow = dtCompanyDetails.NewRow();
            drNewRow["ColumnName"] = "BuyerPhoneNo";
            drNewRow["ColumnData"] = GetMasterDataBasedOnValueName("BuyerPhoneNo"); 
            dtCompanyDetails.Rows.Add(drNewRow);

            return dtCompanyDetails;

        }

        public string GetMasterDataBasedOnValueName(string valueName)
        {
            
            string sMasterValue= lMasterData.Where(p => p.ValueName == valueName).Select(p => p.Value).FirstOrDefault();
            return sMasterValue;
        }
    }
}


