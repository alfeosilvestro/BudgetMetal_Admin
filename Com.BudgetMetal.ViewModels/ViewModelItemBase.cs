using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.ViewModels
{
    public class ViewModelItemBase
    {
        public int Id
        {
            get;
            set;
        }

        public DateTime CreatedDate
        {
            get;
            set;
        }

        public DateTime UpdatedDate
        {
            get;
            set;
        }

        public string CreatedBy
        {
            get;
            set;
        }

        public string UpdatedBy
        {
            get;
            set;
        }

        public bool IsActive
        {
            get;
            set;
        }

        public string Version
        {
            get;
            set;
        }
    }
}
