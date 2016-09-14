using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobManager.Model;
using System.Data;

namespace JobManager.DAL
{
    public class DataAccess
    {
        JobManagerDataContext jmdc = new JobManagerDataContext();
        public DataAccess() { }


        public JobModel GetJobDetails(int jobId){
            var returnedJob = jmdc.GetJobDetails(jobId);
            foreach (GetJobDetailsResult jm in returnedJob)
            {
                JobModel jobDetails = new JobModel();
                jobDetails.JobId = jm.Id;
                jobDetails.JobName = jm.JobName;
                jobDetails.CreatedDate = jm.CreatedDate;

                return jobDetails;
            }
            return null;
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
                jobModel.Materials.Add(jmm);
            }
            return jobModel;
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

        public bool CreateAttribute(AttributeModel objAttributeModel)
        {
            try
            {
                Attribute objAttribute = new Attribute() { Name = objAttributeModel.AttributeName, TypeId = objAttributeModel.AttributeTypeId };
                jmdc.Attributes.InsertOnSubmit(objAttribute);
                jmdc.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;                
            }
           
        }

        public void InsertUpdateJobMaterial(DataTable dtJobMaterial)
        {
            foreach(DataRow dr in dtJobMaterial.Rows)
            {
                jmdc.INSUPDJobMaterial(int.Parse(dr["JobId"].ToString()), int.Parse(dr["MaterialId"].ToString()), int.Parse(dr["AttributeId"].ToString()), int.Parse(dr["ValueId"].ToString()), dr["Value"].ToString());
            }
        }

        public void DeleteJobMaterial(int jobId,int materialId)
        {
            jmdc.DelJobMaterial(jobId, materialId);
        }
    }
}
