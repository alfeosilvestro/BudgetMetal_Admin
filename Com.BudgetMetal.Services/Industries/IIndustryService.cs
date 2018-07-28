using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.Industries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.Services.Industries
{
    public interface IIndustryService
    {
        VmIndustryPage GetIndustriesByPage(string keyword, int page, int totalRecords);

        VmIndustryItem GetIndustryById(int Id);

        VmGenericServiceResult Insert(VmIndustryItem vmIndustryItem);

        VmGenericServiceResult Update(VmIndustryItem IndustryItem);

        void Delete(int Id);
    }
}
