using Com.BudgetMetal.Common;
using Com.BudgetMetal.ViewModels.Company;
using Com.BudgetMetal.ViewModels.Document;
using Com.BudgetMetal.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.ViewModels.Rating
{
    public class VmRatingPage : ViewModelBase
    {
        public PageResult<VmRatingItem> Result { get; set; }
    }
}
