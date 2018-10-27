using Com.BudgetMetal.ViewModels.CodeTable;
using Com.BudgetMetal.ViewModels.Company;
using Com.BudgetMetal.ViewModels.Document;
using Com.BudgetMetal.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.ViewModels.TimeLine
{
    public class VmTimeLineItemForPage
    {
        public string UserName { get; set; }

        public string Message { get; set; }

        public string DocumentNo { get; set; }

        public string DocumentUrl { get; set; }

        public int MessageType { get; set; }

        public string Time { get; set; }

        public bool IsRead { get; set; }
    }
}
