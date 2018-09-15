using Com.BudgetMetal.ViewModels.Document;
using Com.BudgetMetal.ViewModels.Role;
using Com.BudgetMetal.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.ViewModels.DocumentActivity
{
    public class VmDocumentActivityItem : ViewModelItemBase
    {
        public string Action { get; set; }

        public bool IsRfq { get; set; }

        public int Document_Id { get; set; }
        public virtual VmDocumentItem Document { get; set; }

        public int User_Id { get; set; }
        public virtual VmUserItem User { get; set; }
    }
}
