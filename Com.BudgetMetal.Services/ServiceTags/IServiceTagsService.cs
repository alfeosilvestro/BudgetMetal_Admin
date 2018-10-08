using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.ServiceTags;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.Services.ServiceTags
{
    public interface IServiceTagsService 
    {
        Task<VmServiceTagsPage> GetServiceTagsByPage(string keyword, int page, int totalRecords);

        Task<VmServiceTagsItem> GetServiceTagsById(int Id);

        VmGenericServiceResult Insert(VmServiceTagsItem vmServiceTagItem);

        Task<VmGenericServiceResult> Update(VmServiceTagsItem ServiceTagItem);

        Task Delete(int Id);

        Task<VmServiceTagsItem> GetFormObject();

        Task<List<VmServiceTagsItem>> GetVmServiceTagsByIndustry(int Id);

        Task<List<VmServiceTagsItem>> GetActiveVmServiceTags();
    }
}
