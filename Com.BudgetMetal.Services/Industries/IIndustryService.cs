using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.Industries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.Services.Industries
{
    public interface IIndustryService
    {
        Task<VmIndustryPage> GetIndustriesByPage(string keyword, int page, int totalRecords);

        Task<VmIndustryItem> GetIndustryById(int Id);

        VmGenericServiceResult Insert(VmIndustryItem vmIndustryItem);

        Task<VmGenericServiceResult> Update(VmIndustryItem IndustryItem);

        Task Delete(int Id);
    }
}
