using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.EzyTender;
using Com.BudgetMetal.ViewModels.Rfq;

namespace Com.BudgetMetal.Services.RFQ
{
    public interface IRFQService
    {
        Task<VmRfqPage> GetRfqByPage(int documentOwner, int page, int totalRecords);

        string SaveRFQ(VmRfq rfq);

        Task<VmRfqPage> GetRfqByPage(string keyword, int page, int totalRecords);

        Task<VmRfqItem> GetRfqtById(int Id);

        VmGenericServiceResult Insert(VmRfqItem vmItem);

        Task<VmGenericServiceResult> Update(VmRfqItem vmItem);

        Task Delete(int Id);

        Task<VmRfqItem> GetFormObject();

        Task<VmRfq> GetSingleRfqById(int documentId);
    }
}
