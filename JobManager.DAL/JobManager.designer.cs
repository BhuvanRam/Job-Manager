﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JobManager.DAL
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="Global Environ")]
	public partial class JobManagerDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertAttribute(Attribute instance);
    partial void UpdateAttribute(Attribute instance);
    partial void DeleteAttribute(Attribute instance);
    #endregion
		
		public JobManagerDataContext() : 
				base(global::JobManager.DAL.Properties.Settings.Default.Global_EnvironConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public JobManagerDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public JobManagerDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public JobManagerDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public JobManagerDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Attribute> Attributes
		{
			get
			{
				return this.GetTable<Attribute>();
			}
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetMaterialsForJob")]
		public ISingleResult<GetMaterialsForJobResult> GetMaterialsForJob([global::System.Data.Linq.Mapping.ParameterAttribute(Name="JobId", DbType="Int")] System.Nullable<int> jobId)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), jobId);
			return ((ISingleResult<GetMaterialsForJobResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetAttributeValues")]
		public ISingleResult<GetAttributeValuesResult> GetAttributeValues([global::System.Data.Linq.Mapping.ParameterAttribute(Name="AttibuteId", DbType="Int")] System.Nullable<int> attibuteId)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), attibuteId);
			return ((ISingleResult<GetAttributeValuesResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetAttributesOfMaterial")]
		public ISingleResult<GetAttributesOfMaterialResult> GetAttributesOfMaterial([global::System.Data.Linq.Mapping.ParameterAttribute(Name="MaterialId", DbType="Int")] System.Nullable<int> materialId)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), materialId);
			return ((ISingleResult<GetAttributesOfMaterialResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetChildAttributeValues")]
		public ISingleResult<GetChildAttributeValuesResult> GetChildAttributeValues([global::System.Data.Linq.Mapping.ParameterAttribute(Name="ParentAttibuteValueId", DbType="Int")] System.Nullable<int> parentAttibuteValueId)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), parentAttibuteValueId);
			return ((ISingleResult<GetChildAttributeValuesResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.INSUPDJobMaterial")]
		public int INSUPDJobMaterial([global::System.Data.Linq.Mapping.ParameterAttribute(Name="JobId", DbType="Int")] System.Nullable<int> jobId, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="MaterialId", DbType="Int")] System.Nullable<int> materialId, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="AttributeId", DbType="Int")] System.Nullable<int> attributeId, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="ValueId", DbType="Int")] System.Nullable<int> valueId, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Value", DbType="VarChar(200)")] string value)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), jobId, materialId, attributeId, valueId, value);
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.DelJobMaterial")]
		public int DelJobMaterial([global::System.Data.Linq.Mapping.ParameterAttribute(Name="JobId", DbType="Int")] System.Nullable<int> jobId, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="MaterialId", DbType="Int")] System.Nullable<int> materialId)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), jobId, materialId);
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetJobMaterials")]
		public ISingleResult<GetJobMaterialsResult> GetJobMaterials([global::System.Data.Linq.Mapping.ParameterAttribute(Name="JobId", DbType="Int")] System.Nullable<int> jobId)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), jobId);
			return ((ISingleResult<GetJobMaterialsResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetJobDetails")]
		public ISingleResult<GetJobDetailsResult> GetJobDetails([global::System.Data.Linq.Mapping.ParameterAttribute(Name="JobId", DbType="Int")] System.Nullable<int> jobId)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), jobId);
			return ((ISingleResult<GetJobDetailsResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetAllAttributesTypes")]
		public ISingleResult<GetAllAttributesTypesResult> GetAllAttributesTypes()
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())));
			return ((ISingleResult<GetAllAttributesTypesResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetAllAttributesGrid")]
		public ISingleResult<GetAllAttributesGridResult> GetAllAttributesGrid()
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())));
			return ((ISingleResult<GetAllAttributesGridResult>)(result.ReturnValue));
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Attribute")]
	public partial class Attribute : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private string _Name;
		
		private int _TypeId;
		
		private System.Nullable<int> _ParentId;
		
		private EntitySet<Attribute> _Attributes;
		
		private EntityRef<Attribute> _Attribute1;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnTypeIdChanging(int value);
    partial void OnTypeIdChanged();
    partial void OnParentIdChanging(System.Nullable<int> value);
    partial void OnParentIdChanged();
    #endregion
		
		public Attribute()
		{
			this._Attributes = new EntitySet<Attribute>(new Action<Attribute>(this.attach_Attributes), new Action<Attribute>(this.detach_Attributes));
			this._Attribute1 = default(EntityRef<Attribute>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="VarChar(200) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TypeId", DbType="Int NOT NULL")]
		public int TypeId
		{
			get
			{
				return this._TypeId;
			}
			set
			{
				if ((this._TypeId != value))
				{
					this.OnTypeIdChanging(value);
					this.SendPropertyChanging();
					this._TypeId = value;
					this.SendPropertyChanged("TypeId");
					this.OnTypeIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ParentId", DbType="Int")]
		public System.Nullable<int> ParentId
		{
			get
			{
				return this._ParentId;
			}
			set
			{
				if ((this._ParentId != value))
				{
					if (this._Attribute1.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnParentIdChanging(value);
					this.SendPropertyChanging();
					this._ParentId = value;
					this.SendPropertyChanged("ParentId");
					this.OnParentIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Attribute_Attribute", Storage="_Attributes", ThisKey="Id", OtherKey="ParentId")]
		public EntitySet<Attribute> Attributes
		{
			get
			{
				return this._Attributes;
			}
			set
			{
				this._Attributes.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Attribute_Attribute", Storage="_Attribute1", ThisKey="ParentId", OtherKey="Id", IsForeignKey=true)]
		public Attribute Attribute1
		{
			get
			{
				return this._Attribute1.Entity;
			}
			set
			{
				Attribute previousValue = this._Attribute1.Entity;
				if (((previousValue != value) 
							|| (this._Attribute1.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Attribute1.Entity = null;
						previousValue.Attributes.Remove(this);
					}
					this._Attribute1.Entity = value;
					if ((value != null))
					{
						value.Attributes.Add(this);
						this._ParentId = value.Id;
					}
					else
					{
						this._ParentId = default(Nullable<int>);
					}
					this.SendPropertyChanged("Attribute1");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_Attributes(Attribute entity)
		{
			this.SendPropertyChanging();
			entity.Attribute1 = this;
		}
		
		private void detach_Attributes(Attribute entity)
		{
			this.SendPropertyChanging();
			entity.Attribute1 = null;
		}
	}
	
	public partial class GetMaterialsForJobResult
	{
		
		private int _Id;
		
		private string _Name;
		
		public GetMaterialsForJobResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", DbType="Int NOT NULL")]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this._Id = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="VarChar(200) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this._Name = value;
				}
			}
		}
	}
	
	public partial class GetAttributeValuesResult
	{
		
		private int _Id;
		
		private string _Name;
		
		public GetAttributeValuesResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", DbType="Int NOT NULL")]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this._Id = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="VarChar(200) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this._Name = value;
				}
			}
		}
	}
	
	public partial class GetAttributesOfMaterialResult
	{
		
		private int _Id;
		
		private string _Name;
		
		private string _Type;
		
		private int _TypeId;
		
		private System.Nullable<int> _ParentId;
		
		private System.Nullable<int> _HasChilds;
		
		public GetAttributesOfMaterialResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", DbType="Int NOT NULL")]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this._Id = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="VarChar(200) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this._Name = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Type", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string Type
		{
			get
			{
				return this._Type;
			}
			set
			{
				if ((this._Type != value))
				{
					this._Type = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TypeId", DbType="Int NOT NULL")]
		public int TypeId
		{
			get
			{
				return this._TypeId;
			}
			set
			{
				if ((this._TypeId != value))
				{
					this._TypeId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ParentId", DbType="Int")]
		public System.Nullable<int> ParentId
		{
			get
			{
				return this._ParentId;
			}
			set
			{
				if ((this._ParentId != value))
				{
					this._ParentId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_HasChilds", DbType="Int")]
		public System.Nullable<int> HasChilds
		{
			get
			{
				return this._HasChilds;
			}
			set
			{
				if ((this._HasChilds != value))
				{
					this._HasChilds = value;
				}
			}
		}
	}
	
	public partial class GetChildAttributeValuesResult
	{
		
		private int _Id;
		
		private string _Name;
		
		public GetChildAttributeValuesResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", DbType="Int NOT NULL")]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this._Id = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="VarChar(200) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this._Name = value;
				}
			}
		}
	}
	
	public partial class GetJobMaterialsResult
	{
		
		private int _Id;
		
		private string _Name;
		
		private string _Type;
		
		private string _Attributes;
		
		public GetJobMaterialsResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", DbType="Int NOT NULL")]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this._Id = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="VarChar(200) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this._Name = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Type", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string Type
		{
			get
			{
				return this._Type;
			}
			set
			{
				if ((this._Type != value))
				{
					this._Type = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Attributes", DbType="VarChar(MAX)")]
		public string Attributes
		{
			get
			{
				return this._Attributes;
			}
			set
			{
				if ((this._Attributes != value))
				{
					this._Attributes = value;
				}
			}
		}
	}
	
	public partial class GetJobDetailsResult
	{
		
		private int _Id;
		
		private string _JobName;
		
		private System.DateTime _CreatedDate;
		
		public GetJobDetailsResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", DbType="Int NOT NULL")]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this._Id = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_JobName", DbType="VarChar(200) NOT NULL", CanBeNull=false)]
		public string JobName
		{
			get
			{
				return this._JobName;
			}
			set
			{
				if ((this._JobName != value))
				{
					this._JobName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreatedDate", DbType="DateTime NOT NULL")]
		public System.DateTime CreatedDate
		{
			get
			{
				return this._CreatedDate;
			}
			set
			{
				if ((this._CreatedDate != value))
				{
					this._CreatedDate = value;
				}
			}
		}
	}
	
	public partial class GetAllAttributesTypesResult
	{
		
		private int _Id;
		
		private string _Name;
		
		public GetAllAttributesTypesResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", DbType="Int NOT NULL")]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this._Id = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this._Name = value;
				}
			}
		}
	}
	
	public partial class GetAllAttributesGridResult
	{
		
		private int _AttributeId;
		
		private string _AttributeName;
		
		private string _AttributeParentName;
		
		private int _AttributeValueId;
		
		private string _AttributeValueName;
		
		private string _AttributeValueParentName;
		
		public GetAllAttributesGridResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AttributeId", DbType="Int NOT NULL")]
		public int AttributeId
		{
			get
			{
				return this._AttributeId;
			}
			set
			{
				if ((this._AttributeId != value))
				{
					this._AttributeId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AttributeName", DbType="VarChar(200) NOT NULL", CanBeNull=false)]
		public string AttributeName
		{
			get
			{
				return this._AttributeName;
			}
			set
			{
				if ((this._AttributeName != value))
				{
					this._AttributeName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AttributeParentName", DbType="VarChar(200)")]
		public string AttributeParentName
		{
			get
			{
				return this._AttributeParentName;
			}
			set
			{
				if ((this._AttributeParentName != value))
				{
					this._AttributeParentName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AttributeValueId", DbType="Int NOT NULL")]
		public int AttributeValueId
		{
			get
			{
				return this._AttributeValueId;
			}
			set
			{
				if ((this._AttributeValueId != value))
				{
					this._AttributeValueId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AttributeValueName", DbType="VarChar(200) NOT NULL", CanBeNull=false)]
		public string AttributeValueName
		{
			get
			{
				return this._AttributeValueName;
			}
			set
			{
				if ((this._AttributeValueName != value))
				{
					this._AttributeValueName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AttributeValueParentName", DbType="VarChar(200)")]
		public string AttributeValueParentName
		{
			get
			{
				return this._AttributeValueParentName;
			}
			set
			{
				if ((this._AttributeValueParentName != value))
				{
					this._AttributeValueParentName = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
