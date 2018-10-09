using Com.BudgetMetal.ViewModels.Rating;
using Com.BudgetMetal.ViewModels.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.BudgetMetal.ViewModels.Company
{
    public class VmCompanyItem : ViewModelItemBase
    {
        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }

        public string Address { get; set; }

        [Required(ErrorMessage = "Domain is required!")]
        public string Domain { get; set; }

        [Required(ErrorMessage = "Registration number is required!")]
        public string RegNo { get; set; }

        public bool IsVerified { get; set; }
        public int C_BusinessType { get; set; }
        public string About { get; set; }
        public decimal? SupplierAvgRating { get; set; }
        public decimal? BuyerAvgRating { get; set; }
        public int? AwardedQuotation { get; set; }
        public int? SubmittedQuotation { get; set; }
        public int? MaxRFQPerWeek { get; set; }
        public int? MaxQuotationPerWeek { get; set; }
        public virtual List<VmUserItem> UserList { get; set; }
        public virtual List<VmRatingItem> RatingList { get; set; }
    }
}
