using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Manager
{
    public interface IView
    {
        IViewModel ViewModel
        {
            get;
            set;
        }

        void Show();
    }
}
