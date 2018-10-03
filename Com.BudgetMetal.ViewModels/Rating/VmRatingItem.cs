using Com.BudgetMetal.ViewModels.Company;
using Com.BudgetMetal.ViewModels.Document;
using Com.BudgetMetal.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.ViewModels.Rating
{
    public class VmRatingItem
    {
        public int? SpeedOfQuotation { get; set; }
        public int? SpeedofDelivery { get; set; }
        public int? ServiceQuality { get; set; }
        public int? Price { get; set; }
        public int? SpeedofProcessing { get; set; }
        public int? Payment { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Ratingcol { get; set; }
        public string UserName { get; set; }

        public VmCompanyItem Company { get; set; }
        public List<VmCompanyItem> CompanyList { get; set; }

        public VmDocumentItem Document { get; set; }
        public List<VmDocumentItem> DocumentList { get; set; }

        public VmUserItem User { get; set; }
        public List<VmUserItem> UserList { get; set; }        
    }
}
