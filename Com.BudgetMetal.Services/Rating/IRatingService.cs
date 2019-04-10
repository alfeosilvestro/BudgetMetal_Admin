using Com.BudgetMetal.Common;
using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.Rating;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.BudgetMetal.Services.Rating
{
    public interface IRatingService
    {
        Task<VmGenericServiceResult> Insert(VmRatingItem vmRatingItem);

        Task<VmRatingPage> GetRatingData(int page, int totalRecords, int companyId, int statusId, string keyword = "");
    }
}
