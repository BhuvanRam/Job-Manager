using JobManager.DAL;
using JobManager.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Manager.ViewModel
{

    public class AttributeViewModel : INotifyPropertyChanged, IViewModel
    {
        private readonly DelegateCommand _createAttributeCommand;
        public event PropertyChangedEventHandler PropertyChanged;
        private bool isAttributeCreated =true;

        private string _attributeName;
        private int _attributeTypeId;
        private int _attributeTypeValue;
        private string _attributeResult;

        private List<AttributeTypeModel> _lAttributeTypes;
        private AttributeTypeModel _selectedAttributeTypeItem;

        private AttributeTypeModel _selectedEditAttributeTypeItem;

        DataAccess objDataAccess = new DataAccess();
        public AttributeViewModel()
        {
            _createAttributeCommand = new DelegateCommand(CreateAttributeMethod, ResultAttributeCreated);
            LAttributeTypes = objDataAccess.GetAllAttributeTypes();

        }

        private bool ResultAttributeCreated(object obj)
        {
            return isAttributeCreated;
        }

        private void CreateAttributeMethod(object obj)
        {
            AttributeModel objAttributeModel = new AttributeModel() { AttributeName = this.AttributeName, AttributeTypeId = this.SelectedAttributeTypeItem.AttributeTypeId };
            isAttributeCreated = objDataAccess.CreateAttribute(objAttributeModel);
            if (isAttributeCreated)
                AttributeResult = "Attribute created successfully";
            else
                AttributeResult = "Attribute creation failed";
            NotifyPropertyChanged("AttributeResult");
        }

        public DelegateCommand CreateAttributeCommand
        {
            get { return _createAttributeCommand; }
        }




        public string AttributeName
        {
            get
            {
                return _attributeName;
            }

            set
            {
                _attributeName = value;
                NotifyPropertyChanged("AttributeName");
            }
        }

        public int AttributeTypeId
        {
            get
            {
                return _attributeTypeId;
            }

            set
            {
                _attributeTypeId = value;
                NotifyPropertyChanged("AttributeTypeId");
            }
        }

        public int AttributeTypeValue
        {
            get
            {
                return _attributeTypeValue;
            }

            set
            {
                _attributeTypeValue = value;
                NotifyPropertyChanged("AttributeTypeValue");
            }
        }

        public string AttributeResult
        {
            get
            {
                return _attributeResult;
            }

            set
            {
                _attributeResult = value;
                NotifyPropertyChanged("AttributeResult");
            }
        }

        public List<AttributeTypeModel> LAttributeTypes
        {
            get
            {
                return _lAttributeTypes;
            }

            set
            {
                _lAttributeTypes = value;
            }
        }

        public AttributeTypeModel SelectedAttributeTypeItem
        {
            get
            {
                return _selectedAttributeTypeItem;
            }

            set
            {
                _selectedAttributeTypeItem = value;
            }
        }

        public AttributeTypeModel SelectedEditAttributeTypeItem
        {
            get
            {
                return _selectedEditAttributeTypeItem;
            }

            set
            {
                _selectedEditAttributeTypeItem = value;
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
