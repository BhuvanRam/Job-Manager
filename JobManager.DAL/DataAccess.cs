using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobManager.Model;
using System.Data;
using System.Collections;
using System.Configuration;

namespace JobManager.DAL
{
    public class DataAccess
    {
        JobManagerDataContext jmdc;
        public DataAccess()
        {
            string connectionString = ConfigurationManager.AppSettings["Global_EnvironConnectionString"];
            jmdc = new JobManagerDataContext(connectionString);
        }


        public JobModel GetJobDetails(int jobId)
        {
            var returnedJob = jmdc.GetJobDetails(jobId);
            foreach (GetJobDetailsResult jm in returnedJob)
            {
                JobModel jobDetails = new JobModel();
                jobDetails.JobId = jm.Id;
                jobDetails.JobName = jm.JobName;
                jobDetails.CreatedDate = jm.CreatedDate;
                jobDetails.StatusId = jm.StatusId;
                jobDetails.BranchId = jm.BranchId;
                return jobDetails;
            }
            return null;
        }


        public void SaveJobDetails(JobModel job)
        {
            jmdc.UPDJobDetails(job.JobId, job.JobName, job.StatusId, job.BranchId);
        }

        public JobModel GetJobMaterials(int jobId)
        {
            JobModel jobModel = new JobModel();
            jobModel.JobId = jobId;
            jobModel.Materials = new List<JobMaterialModel>();
            var JobMaterials = jmdc.GetJobMaterials(jobId);

            foreach (GetJobMaterialsResult jm in JobMaterials)
            {
                JobMaterialModel jmm = new JobMaterialModel();
                jmm.Id = jm.Id;
                jmm.Name = jm.Name;
                jmm.Attributes = jm.Attributes;
                jmm.Type = jm.Type;
                jmm.Quantity = jm.Quantity;
                if (jm.POId.HasValue)
                {
                    jmm.POId = Convert.ToInt32(jm.POId);
                    jmm.IsSelected = true;
                }
                else
                {
                    jmm.POId = null;
                    jmm.IsSelected = false;
                }
                jmm.OrderedBy = jm.OrderedBy;
                if (jm.OrderedOn.HasValue)
                    jmm.OrderedOn = Convert.ToDateTime(jm.OrderedOn);
                else
                    jmm.OrderedOn = null;
                jobModel.Materials.Add(jmm);
            }
            return jobModel;
        }
        public List<JobPOMaterialModel> GetJobPOMaterials(int jobId, int? poId)
        {

            List<JobPOMaterialModel> Materials = new List<JobPOMaterialModel>();
            var JobMaterials = jmdc.GetJobPOMaterials(jobId, poId);

            foreach (GetJobPOMaterialsResult jm in JobMaterials)
            {
                JobPOMaterialModel jmm = new JobPOMaterialModel();
                jmm.Id = jm.Id;
                jmm.Name = jm.Name;
                jmm.Attributes = jm.Attributes;
                jmm.Type = jm.Type;
                jmm.RequiredQuantity = jm.RequiredQuantity;
                if (jm.Quantity.HasValue)
                    jmm.Quantity = Convert.ToInt32(jm.Quantity);
                if (jm.UnitPrice.HasValue)
                    jmm.Price = Convert.ToDecimal(jm.UnitPrice);
                Materials.Add(jmm);
            }
            return Materials;
        }
        public List<MaterialModel> GetMaterialsForJob(int jobId)
        {
            List<MaterialModel> materialModels = new List<MaterialModel>();
            materialModels.Add(new MaterialModel(-1, "Select"));
            var materialsForJob = jmdc.GetMaterialsForJob(jobId);

            foreach (GetMaterialsForJobResult jm in materialsForJob)
            {
                MaterialModel mm = new MaterialModel();
                mm.Id = jm.Id;
                mm.Name = jm.Name;

                materialModels.Add(mm);
            }
            return materialModels;
        }
        public List<MaterialAttributeModel> GetAttributesOfMaterial(int materialId)
        {
            List<MaterialAttributeModel> attributes = new List<MaterialAttributeModel>();

            var attributesOfMaterial = jmdc.GetAttributesOfMaterial(materialId);

            foreach (GetAttributesOfMaterialResult jm in attributesOfMaterial)
            {
                MaterialAttributeModel ma = new MaterialAttributeModel();
                ma.Id = jm.Id;
                ma.Name = jm.Name;
                ma.Type = jm.Type;
                ma.TypeId = jm.TypeId;
                ma.ParentId = jm.ParentId;
                ma.HasChilds = jm.HasChilds;
                attributes.Add(ma);
            }
            return attributes;
        }

        public List<AttributeValueModel> GetAttributeValues(int attributeId)
        {
            List<AttributeValueModel> attribValues = new List<AttributeValueModel>();
            attribValues.Add(new AttributeValueModel(-1, "Select"));
            var values = jmdc.GetAttributeValues(attributeId);

            foreach (GetAttributeValuesResult jm in values)
            {
                AttributeValueModel ma = new AttributeValueModel();
                ma.Id = jm.Id;
                ma.Name = jm.Name;
                attribValues.Add(ma);
            }
            return attribValues;
        }
        public List<AttributeValueModel> GetChildAttributeValues(int parentAttributeValueId)
        {
            List<AttributeValueModel> attribValues = new List<AttributeValueModel>();
            attribValues.Add(new AttributeValueModel(-1, "Select"));
            var values = jmdc.GetChildAttributeValues(parentAttributeValueId);

            foreach (GetChildAttributeValuesResult jm in values)
            {
                AttributeValueModel ma = new AttributeValueModel();
                ma.Id = jm.Id;
                ma.Name = jm.Name;
                attribValues.Add(ma);
            }
            return attribValues;
        }

        public List<AttributeTypeModel> GetAllAttributeTypes()
        {
            List<AttributeTypeModel> attribValues = new List<AttributeTypeModel>();
            var result = jmdc.GetAllAttributesTypes().Select(p => new { AttributeTypeId = p.Id, AttributeTypeName = p.Name });
            result.ToList().ForEach(p => { attribValues.Add(new AttributeTypeModel() { AttributeTypeId = p.AttributeTypeId, AttributeTypeName = p.AttributeTypeName }); });
            return attribValues;
        }

        public List<AttributeModel> GetAllAttributes()
        {
            List<AttributeModel> lAttributes = new List<AttributeModel>();
            var result = jmdc.Attributes.Select(p => new { AttributeName = p.Name, AttributeId = p.Id, AttributeTypeId = p.TypeId, ParentId = p.ParentId });
            result.ToList().ForEach(p => { lAttributes.Add(new AttributeModel() { AttributeId = p.AttributeId, AttributeName = p.AttributeName, AttributeTypeId = p.AttributeTypeId, ParentId = Convert.ToInt32(p.ParentId) }); });
            return lAttributes;
        }

        public AttributeModel GetAttributeByAttributeId(int AttributeId)
        {
            var result = jmdc.Attributes.Where(p => p.Id == AttributeId).Select(p => p).SingleOrDefault();
            AttributeModel objAttributeModel = new AttributeModel() { AttributeId = result.Id, AttributeName = result.Name, ParentId = Convert.ToInt32(result.ParentId) };
            return objAttributeModel;
        }

        public List<AttributeModel> GetAttributeByType(int iAttributeTypeId)
        {
            List<AttributeModel> lAttributes = new List<AttributeModel>();
            var result = jmdc.Attributes.Where(p => p.TypeId == iAttributeTypeId).Select(p => new { AttributeName = p.Name, AttributeId = p.Id });

            foreach (var item in result)
            {
                lAttributes.Add(new AttributeModel() { AttributeId = item.AttributeId, AttributeName = item.AttributeName, AttributeTypeId = 2 });
            }
            return lAttributes;
        }

        public List<AttributeValueModel> GetAttributeValueByAttributeId(int iAttributeId)
        {
            List<AttributeValueModel> lAttributeValueModels = new List<AttributeValueModel>();
            var result = jmdc.AttributeValues.Where(p => p.AttributeId == iAttributeId).Select(p => new { AttributeValueame = p.Name, AttributeValueId = p.Id, AttributeId = p.AttributeId, AttibuteValueParentAttribute = p.ParentValue });
            foreach (var item in result)
                lAttributeValueModels.Add(new AttributeValueModel()
                {
                    Id = item.AttributeValueId,
                    Name = item.AttributeValueame,
                    AttributeId = item.AttributeId,
                    ParentValue = item.AttibuteValueParentAttribute == null ? 0 : (int)item.AttibuteValueParentAttribute
                });
            return lAttributeValueModels;
        }

        public List<AttributeValueModel> GetAttributeValueByParentId(int iParentId)
        {
            List<AttributeValueModel> lAttributeValueModels = new List<AttributeValueModel>();
            var result = jmdc.AttributeValues.Where(p => p.ParentValue == iParentId).Select(p => new { AttributeValueName = p.Name, AttributeValueId = p.Id, AttributeID = p.AttributeId, ParentValue = p.ParentValue });
            foreach (var item in result)
                lAttributeValueModels.Add(new AttributeValueModel() { Id = item.AttributeValueId, Name = item.AttributeValueName, AttributeId = item.AttributeID, ParentValue = (item.ParentValue == null ? 0 : (int)item.ParentValue) });
            return lAttributeValueModels;
        }



        public void InsertUpdateJobMaterial(DataTable dtJobMaterial, JobMaterialField jmf)
        {
            foreach (DataRow dr in dtJobMaterial.Rows)
            {
                jmdc.INSUPDJobMaterial(int.Parse(dr["JobId"].ToString()), int.Parse(dr["MaterialId"].ToString()), int.Parse(dr["AttributeId"].ToString()), int.Parse(dr["ValueId"].ToString()), dr["Value"].ToString());
            }

            jmdc.JobMaterialFields.InsertOnSubmit(jmf);
            jmdc.SubmitChanges();
        }

        public void DeleteJobMaterial(int jobId, int materialId)
        {
            jmdc.DelJobMaterial(jobId, materialId);
        }

        public List<AttributeGridModel> GetAttributeGridDetails()
        {
            List<AttributeGridModel> lAttributeGridModel = new List<AttributeGridModel>();
            var resultGrid = jmdc.GetAllAttributesGrid();
            foreach (var item in resultGrid)
            {
                AttributeGridModel objAttributeGridModel = new AttributeGridModel();
                objAttributeGridModel.AttributeId = item.AttributeId;
                objAttributeGridModel.AttributeName = item.AttributeName;
                objAttributeGridModel.AttributeParentName = item.AttributeParentName;
                objAttributeGridModel.AttributeValueId = item.AttributeValueId;
                objAttributeGridModel.AttributeValueName = item.AttributeValueName;
                objAttributeGridModel.AttributeValueParentName = item.AttributeValueParentName;
                lAttributeGridModel.Add(objAttributeGridModel);
            }
            return lAttributeGridModel;
        }

        public bool AddAttributeValueForPlainText(string attributeValue, int attributeId)
        {
            bool isResult = true;
            var isCheckAttributeValue = jmdc.AttributeValues.Where(p => p.Id == attributeId);
            if (!isCheckAttributeValue.Any())
            {
                AttributeValue objInsertAttributeValue = new AttributeValue() { Name = attributeValue, AttributeId = attributeId };
                jmdc.AttributeValues.InsertOnSubmit(objInsertAttributeValue);
                jmdc.SubmitChanges();
            }
            else
            {
                AttributeValue objUpdateAttributeValue = isCheckAttributeValue.SingleOrDefault();
                objUpdateAttributeValue.Name = attributeValue;
                jmdc.AttributeValues.InsertOnSubmit(objUpdateAttributeValue);
                jmdc.SubmitChanges();
            }
            return isResult;
        }

        public AttributeValueModel AddAttributeValue(AttributeValueModel objAttributeModel)
        {
            try
            {
                int? iParentId = objAttributeModel.ParentValue == 0 ? null : (int?)objAttributeModel.ParentValue;
                AttributeValue objAttributeValue = new AttributeValue() { AttributeId = objAttributeModel.AttributeId, Name = objAttributeModel.Name, ParentValue = iParentId };
                jmdc.AttributeValues.InsertOnSubmit(objAttributeValue);
                jmdc.SubmitChanges();
                objAttributeModel.AttributeId = objAttributeValue.AttributeId;
                objAttributeModel.Name = objAttributeValue.Name;
                objAttributeModel.Id = objAttributeValue.Id;
                objAttributeModel.ParentValue = objAttributeValue.ParentValue == null ? 0 : (int)objAttributeValue.ParentValue;
                return objAttributeModel;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool AddAttributeValueList(List<AttributeValueModel> attributeValueModel)
        {
            bool isResult = true;
            List<AttributeValue> lAttributeValue = new List<AttributeValue>();
            foreach (AttributeValueModel item in attributeValueModel)
            {
                int? iParentId = item.ParentValue == 0 ? null : (int?)item.ParentValue;
                AttributeValue objInsertAttributeValue = new AttributeValue() { Name = item.Name, AttributeId = item.AttributeId, ParentValue = iParentId };
                lAttributeValue.Add(objInsertAttributeValue);
            }
            jmdc.AttributeValues.InsertAllOnSubmit(lAttributeValue);
            jmdc.SubmitChanges();
            return isResult;
        }

        public bool UpdateAttributeValueList(List<AttributeValueModel> attributeValueModel)
        {
            bool isResult = true;
            try
            {
                if (jmdc.Connection.State == ConnectionState.Closed)
                    jmdc.Connection.Open();
                jmdc.Transaction = jmdc.Connection.BeginTransaction();
                foreach (AttributeValueModel item in attributeValueModel)
                {
                    int? iParentId = item.ParentValue == 0 ? null : (int?)item.ParentValue;
                    AttributeValue retreievedAttributeValue = jmdc.AttributeValues.Where(p => p.Id == item.Id).SingleOrDefault();
                    retreievedAttributeValue.AttributeId = item.AttributeId;
                    retreievedAttributeValue.Name = item.Name;
                    retreievedAttributeValue.ParentValue = iParentId;
                    jmdc.SubmitChanges();
                }
                jmdc.Transaction.Commit();
            }
            catch (Exception ex)
            {
                jmdc.Transaction.Rollback();
                isResult = false;
                throw ex;
            }
            finally
            {
                if (jmdc.Connection.State == ConnectionState.Open)
                    jmdc.Connection.Close();
            }
            return isResult;
        }

        public bool DeleteAttributeValue(AttributeValueModel attributeValueModel)
        {
            bool isResult = true;
            try
            {
                AttributeValue deleteAttributeValue = jmdc.AttributeValues.Where(p => p.Id == attributeValueModel.Id).SingleOrDefault();
                jmdc.AttributeValues.DeleteOnSubmit(deleteAttributeValue);
                jmdc.SubmitChanges();
            }
            catch (Exception ex)
            {
                isResult = false;
                throw ex;
            }
            return isResult;
        }

        public bool DeleteAttributeValueList(List<AttributeValueModel> lAttributeValueModel)
        {
            bool isResult = true;
            try
            {
                List<AttributeValue> lDeleteAttributeValue = new List<AttributeValue>();
                foreach (AttributeValueModel attributeValueModel in lAttributeValueModel)
                {
                    AttributeValue deleteAttributeValue = jmdc.AttributeValues.Where(p => p.Id == attributeValueModel.Id).SingleOrDefault();
                    lDeleteAttributeValue.Add(deleteAttributeValue);
                }

                jmdc.AttributeValues.DeleteAllOnSubmit(lDeleteAttributeValue);
                jmdc.SubmitChanges();
            }
            catch (Exception ex)
            {
                isResult = false;
                throw ex;
            }
            return isResult;
        }

        public bool AddAttribute(AttributeModel objAttributeModel)
        {
            try
            {
                int? iParentId = objAttributeModel.ParentId == 0 ? null : (int?)objAttributeModel.ParentId;
                Attribute objAttribute = new Attribute() { Name = objAttributeModel.AttributeName, TypeId = objAttributeModel.AttributeTypeId, ParentId = iParentId };
                jmdc.Attributes.InsertOnSubmit(objAttribute);
                jmdc.SubmitChanges();
                objAttributeModel.AttributeId = objAttribute.Id;
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool ModifyAttribute(AttributeModel objAttributeModel)
        {
            try
            {
                int? iParentId = objAttributeModel.ParentId == 0 ? null : (int?)objAttributeModel.ParentId;
                Attribute objAttribute = jmdc.Attributes.Where(p => p.Id == objAttributeModel.AttributeId).SingleOrDefault();
                objAttribute.Name = objAttributeModel.AttributeName;
                objAttribute.ParentId = iParentId;
                jmdc.SubmitChanges();
                objAttributeModel.AttributeId = objAttribute.Id;
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public AttributeTypeModel GetAttributeTypeById(int iAttributeTypeId)
        {
            try
            {
                var result = jmdc.GetAllAttributesTypes().Where(p => p.Id == iAttributeTypeId).Select(p => new { AttributeTypeId = p.Id, AttributeTypeName = p.Name }).SingleOrDefault();
                AttributeTypeModel objAttributeTypeModel = new AttributeTypeModel() { AttributeTypeId = result.AttributeTypeId, AttributeTypeName = result.AttributeTypeName };
                return objAttributeTypeModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteAttribute(int iAttributeId)
        {
            bool isResult = true;
            try
            {
                var attributeToBeDeleted = jmdc.Attributes.Where(p => p.Id == iAttributeId).Select(p => p);
                if (attributeToBeDeleted.Any())
                {
                    jmdc.Attributes.DeleteOnSubmit(attributeToBeDeleted.SingleOrDefault());
                    jmdc.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                isResult = false;
                throw ex;
            }
            return isResult;
        }

        public List<MaterialType> GetMaterialTypes()
        {
            List<MaterialType> lMaterialTypes = new List<MaterialType>();
            lMaterialTypes = jmdc.MaterialTypes.ToList();
            return lMaterialTypes;
        }

        public List<MaterialAttribute> GetMaterialAttributes()
        {
            var MaterialAttributes = jmdc.MaterialAttributes.Select(p => p).ToList();
            return MaterialAttributes;
        }

        public List<MaterialAttribute> GetMaterialAttributesByMaterialId(int MaterialId)
        {
            List<MaterialAttribute> MaterialAttribute = jmdc.MaterialAttributes.Where(p => p.MaterialId == MaterialId).OrderBy(p => p.SortOder).ToList();
            return MaterialAttribute;
        }

        public List<Material> GetMaterials()
        {
            var Materials = jmdc.Materials.Select(p => p).ToList();
            return Materials;
        }

        public bool SaveMaterial(Material objSaveMaterial)
        {
            bool isResult = true;
            try
            {
                jmdc.Materials.InsertOnSubmit(objSaveMaterial);
                jmdc.SubmitChanges();
            }
            catch (Exception ex)
            {
                return false;
            }
            return isResult;
        }

        public bool DeleteMaterial(int MaterialId)
        {
            bool isResult = true;
            try
            {
                List<MaterialAttribute> lMaterialAttributes = jmdc.MaterialAttributes.Where(p => p.MaterialId == MaterialId).Select(p => p).ToList();
                foreach (MaterialAttribute itemMaterialAttribute in lMaterialAttributes)
                {
                    DeleteAttributeFromMaterial(MaterialId, itemMaterialAttribute.AttributeId);
                }
                Material objDeleteMaterial = jmdc.Materials.Where(p => p.Id == MaterialId).Select(p => p).FirstOrDefault();
                jmdc.Materials.DeleteOnSubmit(objDeleteMaterial);
                jmdc.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
            return isResult;
        }

        public bool DeleteAttributeFromMaterial(int MaterialId, int AttributeId)
        {
            bool isResult = true;
            try
            {
                MaterialAttribute objMaterialAttribute = jmdc.MaterialAttributes.Where(p => p.MaterialId == MaterialId && p.AttributeId == AttributeId).Select(p => p).FirstOrDefault();
                if (objMaterialAttribute != null)
                {
                    jmdc.MaterialAttributes.DeleteOnSubmit(objMaterialAttribute);
                    jmdc.SubmitChanges();
                }
            }
            catch (Exception)
            {
                isResult = false;
                throw;
            }
            return isResult;
        }

        public bool UpdateMaterial(Material objUpdateMaterial)
        {
            bool isUpdate = true;
            try
            {
                Material objDBMaterial = jmdc.Materials.Where(p => p.Id == objUpdateMaterial.Id).Select(p => p).FirstOrDefault();
                MaterialType objDBMaterialType = jmdc.MaterialTypes.Where(p => p.Id == objUpdateMaterial.TypeId).Select(p => p).FirstOrDefault();
                objDBMaterial.MaterialType = objDBMaterialType;
                //objDBMaterial.TypeId = objUpdateMaterial.TypeId;
                objDBMaterial.IsActive = objUpdateMaterial.IsActive;
                jmdc.SubmitChanges();
            }
            catch (Exception)
            {
                isUpdate = false;
                return isUpdate;
            }
            return isUpdate;
        }

        public bool AddMaterial(Material objMaterial)
        {
            bool isInserted = true;
            try
            {
                jmdc.Materials.InsertOnSubmit(objMaterial);
                jmdc.SubmitChanges();
            }
            catch (Exception)
            {
                isInserted = false;
                return isInserted;
            }
            return isInserted;
        }

        public bool UpdateMaterialAttributes(List<MaterialAttribute> lMaterialAttributes)
        {
            bool isUpdate = true;
            try
            {
                foreach (MaterialAttribute itemMaterialAttribute in lMaterialAttributes)
                {
                    var materialAttributes = jmdc.MaterialAttributes.Where(p => p.MaterialId == itemMaterialAttribute.MaterialId && p.AttributeId == itemMaterialAttribute.AttributeId).Select(p => p);
                    if (materialAttributes.Any() == false)
                    {
                        jmdc.MaterialAttributes.InsertOnSubmit(itemMaterialAttribute);
                        jmdc.SubmitChanges();
                    }
                    else
                    {
                        MaterialAttribute selectedMaterialAttribute = materialAttributes.FirstOrDefault();
                        selectedMaterialAttribute.MaterialId = itemMaterialAttribute.MaterialId;
                        selectedMaterialAttribute.AttributeId = itemMaterialAttribute.AttributeId;
                        selectedMaterialAttribute.SortOder = itemMaterialAttribute.SortOder;
                        selectedMaterialAttribute.IsRequired = itemMaterialAttribute.IsRequired;
                        jmdc.SubmitChanges();
                    }
                }
            }
            catch (Exception)
            {
                isUpdate = false;
                return isUpdate;
            }

            return isUpdate;
        }

        public bool AddMaterialAttributes(List<MaterialAttribute> lMaterialAttributes)
        {
            bool isUpdate = true;
            try
            {
                jmdc.MaterialAttributes.InsertAllOnSubmit(lMaterialAttributes);
                jmdc.SubmitChanges();
            }
            catch (Exception)
            {
                isUpdate = false;
                return isUpdate;
            }

            return isUpdate;
        }

        public List<State> GetStates()
        {
            List<State> lStates = new List<State>();
            try
            {
                lStates = jmdc.States.Select(p => p).ToList();
            }
            catch (Exception)
            {

                throw;
            }
            return lStates;
        }

        public bool AddVendor(Vendor objInsertVendor)
        {
            bool isInserted = false;
            try
            {
                jmdc.Vendors.InsertOnSubmit(objInsertVendor);
                jmdc.SubmitChanges();
                isInserted = true;
            }
            catch (Exception)
            {
                isInserted = false;
            }
            return isInserted;
        }
        public int AddJob(Job objJob)
        {
            try
            {
                jmdc.Jobs.InsertOnSubmit(objJob);
                jmdc.SubmitChanges();
                int newJobId = jmdc.Jobs.OrderByDescending(job => job.Id).First().Id;
                return newJobId;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public bool DeleteVendor(string VendorCode)
        {
            bool isDeleted = false;
            try
            {
                Vendor objDeleteVendor = jmdc.Vendors.Where(p => p.VendorCode == VendorCode).Select(p => p).FirstOrDefault();
                jmdc.Vendors.DeleteOnSubmit(objDeleteVendor);
                jmdc.SubmitChanges();
                isDeleted = true;
            }
            catch (Exception)
            {
                return isDeleted;
            }
            return isDeleted;
        }

        public bool UpdateVendor(Vendor objUpdateVendor)
        {
            bool isUpdated = false;
            try
            {
                Vendor objVendor = jmdc.Vendors.Where(p => p.VendorCode == objUpdateVendor.VendorCode).Select(p => p).FirstOrDefault();
                objVendor.VendorName = objUpdateVendor.VendorName;
                objVendor.Address1 = objUpdateVendor.Address1;
                objVendor.Address2 = objUpdateVendor.Address2;
                objVendor.City = objUpdateVendor.City;
                objVendor.StateId = objUpdateVendor.StateId;
                objVendor.PIN = objUpdateVendor.PIN;
                objVendor.PhoneNo = objUpdateVendor.PhoneNo;
                objVendor.FaxNo = objUpdateVendor.FaxNo;
                objVendor.Email = objUpdateVendor.Email;
                objVendor.ContactPerson = objUpdateVendor.ContactPerson;
                jmdc.SubmitChanges();
                isUpdated = true;
            }
            catch (Exception)
            {
                return isUpdated;
            }
            return isUpdated;
        }

        public Vendor GetVendor(string VendorCode)
        {
            Vendor objGetVendor = new Vendor();
            try
            {
                objGetVendor = jmdc.Vendors.Where(p => p.VendorCode == VendorCode).Select(p => p).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            return objGetVendor;
        }

        public Vendor GetVendorById(int iVendorId)
        {
            Vendor objGetVendor = new Vendor();
            try
            {
                objGetVendor = jmdc.Vendors.Where(p => p.VendorId== iVendorId).Select(p => p).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            return objGetVendor;
        }


        public List<Vendor> GetVendorByName(string VendorName)
        {
            List<Vendor> lGetVendors = new List<Vendor>();
            try
            {
                lGetVendors = jmdc.Vendors.Where(p => p.VendorName.Contains(VendorName)).Select(p => p).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return lGetVendors;
        }

        public List<Vendor> GetAllVendors()
        {
            List<Vendor> lGetVendors = new List<Vendor>();
            try
            {
                lGetVendors = jmdc.Vendors.Select(p => p).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return lGetVendors;
        }

        public USER AuthenticateUser(string userName, string Password)
        {
            USER objAuthenticateUser = new USER();
            try
            {
                var AuthenticateUser = jmdc.USERs.Where(p => p.UserName == userName && p.Password == Password);
                if (AuthenticateUser.Any())
                {
                    objAuthenticateUser = AuthenticateUser.SingleOrDefault();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objAuthenticateUser;
        }

        public List<USER> GetAllUsers()
        {
            List<USER> lUsers = jmdc.USERs.Select(p => p).ToList();
            return lUsers;
        }

        public List<ROLE> GetAllRoles()
        {
            List<ROLE> lRoles = jmdc.ROLEs.Select(p => p).ToList();
            return lRoles;
        }

        public bool CheckUserNameAvailability(string userName)
        {
            var userDetails = jmdc.USERs.Where(p => p.UserName == userName);
            return userDetails.Any();
        }

        public bool saveUser(USER objnewUser)
        {
            bool isInserted = false;
            try
            {
                jmdc.USERs.InsertOnSubmit(objnewUser);
                jmdc.SubmitChanges();
                isInserted = true;
            }
            catch (Exception)
            {
                isInserted = false;
            }
            return isInserted;

        }

        public bool DeleteUser(int userId)
        {
            bool isDeleted = false;
            try
            {
                USER objDeleteUSER = jmdc.USERs.Where(p => p.UserID == userId).Select(p => p).FirstOrDefault();
                jmdc.USERs.DeleteOnSubmit(objDeleteUSER);
                jmdc.SubmitChanges();
                isDeleted = true;
            }
            catch (Exception)
            {
                return isDeleted;
            }
            return isDeleted;
        }
        public List<JobManager.Model.JobStatus> GetJobStatuses()
        {
            var jobStatuses = jmdc.JobStatus.Select(p => p).ToList();
            List<JobManager.Model.JobStatus> lJobStatus = new List<JobManager.Model.JobStatus>();
            foreach (var item in jobStatuses)
            {
                JobManager.Model.JobStatus objJobStatus = new JobManager.Model.JobStatus();
                objJobStatus.Id = item.Id;
                objJobStatus.Name = item.Name;
                lJobStatus.Add(objJobStatus);
            }
            return lJobStatus;
        }
        public List<JobManager.Model.BranchModel> GetBranches()
        {
            var branches = jmdc.GetBraches();

            List<JobManager.Model.BranchModel> lBranches = new List<JobManager.Model.BranchModel>();
            foreach (var item in branches)
            {
                JobManager.Model.BranchModel branch = new JobManager.Model.BranchModel();
                branch.Id = item.Id;
                branch.Name = item.Name;
                lBranches.Add(branch);
            }
            return lBranches;
        }
        public List<JobManager.Model.JobModel> SearchJobs(int? jobId, int? branchId, DateTime? startDate, DateTime? endDate)
        {
            var jobs = jmdc.SearchJobDetails(jobId, branchId, startDate, endDate);

            List<JobManager.Model.JobModel> jobsList = new List<JobManager.Model.JobModel>();
            foreach (var jm in jobs)
            {
                JobModel jobDetails = new JobModel();
                jobDetails.JobId = jm.Id;
                jobDetails.JobName = jm.JobName;
                jobDetails.CreatedDate = jm.CreatedDate;
                jobDetails.StatusId = jm.StatusId;
                jobDetails.BranchId = jm.BranchId;
                jobDetails.Status = jm.Status;
                jobDetails.Branch = jm.Branch;
                jobsList.Add(jobDetails);
            }
            return jobsList;
        }

        public int InsJobPO(JobPOModel jobPO)
        {
            var result = jmdc.INSJobPO(jobPO.JobId, jobPO.CreatedById, jobPO.VendorId, jobPO.Discount, jobPO.Delivery, jobPO.Payment, jobPO.Packing, jobPO.ExciseDuty, jobPO.TaxesExtra, jobPO.TransportInsurance, jobPO.Transportation, jobPO.Octroi);
            return Convert.ToInt32(result.Single<INSJobPOResult>().Column1);
        }
        public void InsJobPOMaterila(int POId, int jobId, JobPOMaterialModel jobPOMaterial)
        {
            jmdc.INSJobPOMaterial(POId, jobId, jobPOMaterial.Id, jobPOMaterial.Quantity, jobPOMaterial.Price);
        }

        public JobPOModel GetJobPODetails(int POId)
        {
            var returnedJobPO = jmdc.GetJobPODetails(POId);
            foreach (GetJobPODetailsResult jm in returnedJobPO)
            {
                JobPOModel jobPODetails = new JobPOModel();
                jobPODetails.JobId = jm.JobId;
                jobPODetails.POId = jm.Id;
                jobPODetails.VendorId = jm.VendorId;
                jobPODetails.StatusId = jm.StatusId;
                jobPODetails.CreatedBy = jm.CreatedBy;
                jobPODetails.CreatedOn = jm.CreatedOn;
                jobPODetails.ApprovedById = jm.ApprovedById;
                jobPODetails.ApprovedBy = jm.ApprovedBy;
                jobPODetails.ApprovedOn = jm.ApprovedOn;
                jobPODetails.Discount = jm.Discount;
                jobPODetails.Delivery = jm.Delivery;
                jobPODetails.Payment = jm.Payment;
                jobPODetails.Packing = jm.Packing;
                jobPODetails.ExciseDuty = jm.ExciseDuty;
                jobPODetails.TaxesExtra = jm.TaxesExtra;
                jobPODetails.TransportInsurance = jm.TransitInsurance;
                jobPODetails.Transportation = jm.Transportation;
                jobPODetails.Octroi = jm.Octroi;
                jobPODetails.PONumber = jm.Fiscal_Year.ToString() + "/" + jm.Id + "/" + jm.JobId;
                return jobPODetails;
            }
            return null;
        }
        public List<JobPOStatus> GetJobPOStatuses()
        {
            var jobStatuses = jmdc.GetJobPOStatuses();
            List<JobPOStatus> lJobStatus = new List<JobPOStatus>();
            foreach (var item in jobStatuses)
            {
                JobPOStatus objJobStatus = new JobPOStatus();
                objJobStatus.Id = item.Id;
                objJobStatus.Name = item.Name;
                lJobStatus.Add(objJobStatus);
            }
            return lJobStatus;
        }

        public void DelPOMaterials(int POId)
        {
            jmdc.DelJobPOMaterial(POId);
        }
        public void UpdJobPO(JobPOModel jobPO)
        {
            jmdc.UPDJobPODetails(jobPO.POId, jobPO.ModifiedById, jobPO.VendorId, jobPO.StatusId, jobPO.ApprovedById, jobPO.ApprovedOn, jobPO.Discount, jobPO.Delivery, jobPO.Payment, jobPO.Packing, jobPO.ExciseDuty, jobPO.TaxesExtra, jobPO.TransportInsurance, jobPO.Transportation, jobPO.Octroi);
        }

        public PurchaseOrder GetPurchaseOrderByJobId(int iJobId)
        {
            PurchaseOrder objPurchaseOrder = jmdc.PurchaseOrders.Where(p => p.JobId == iJobId).FirstOrDefault();
            return objPurchaseOrder;
        }

        public PurchaseOrder GetPurchaseOrderById(int iPOId)
        {
            PurchaseOrder objPurchaseOrder = jmdc.PurchaseOrders.Where(p => p.Id == iPOId).FirstOrDefault();
            return objPurchaseOrder;
        }

        public List<MasterData> GetMasterData(string groupName)
        {
            List<MasterData> lMasterData = jmdc.MasterDatas.Where(p => p.GroupName == groupName).ToList<MasterData>();
            return lMasterData;
        }

        public Branch GetBranchDetails(int BranchId)
        {
            Branch brDetail = jmdc.Branches.Where(p => p.Id == BranchId).Select(p => p).FirstOrDefault();
            return brDetail;
        }
    }

  
}
