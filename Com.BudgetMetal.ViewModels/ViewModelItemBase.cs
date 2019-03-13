using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MMM.yyyy}")]
        public DateTime CreatedDate
        {
            get;
            set;
        }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MMM.yyyy}")]
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
