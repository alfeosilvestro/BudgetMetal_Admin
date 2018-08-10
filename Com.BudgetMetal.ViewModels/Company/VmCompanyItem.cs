using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.ViewModels.Company
{
    public class VmCompanyItem : ViewModelItemBase
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Domain { get; set; }
        public string RegNo { get; set; }
        public bool IsVerified { get; set; }
        public string About { get; set; }
        public decimal? SupplierAvgRating { get; set; }
        public decimal? BuyerAvgRating { get; set; }
        public int? AwardedQuotation { get; set; }
        public int? SubmittedQuotation { get; set; }
    }
}
