using Com.BudgetMetal.ViewModels.CodeTable;
using Com.BudgetMetal.ViewModels.Company;
using Com.BudgetMetal.ViewModels.Document;
using Com.BudgetMetal.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.ViewModels.TimeLine
{
    public class VmTimeLineItem : ViewModelItemBase
    {
        
        public int Company_Id { get; set; }
        public virtual VmCompanyItem Company { get; set; }

       
        public int User_Id { get; set; }
        public virtual VmUserItem User { get; set; }


        public string Message { get; set; }

      
        public int Document_Id { get; set; }
        public virtual VmDocumentItem Document { get; set; }

       
        public int MessageType { get; set; }
        public virtual VmCodeTableItem MType { get; set; }

        public bool IsRead { get; set; }
    }
}
