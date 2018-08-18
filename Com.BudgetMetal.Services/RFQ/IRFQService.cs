using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Com.BudgetMetal.ViewModels.EzyTender;
using Com.BudgetMetal.ViewModels.Rfq;

namespace Com.BudgetMetal.Services.RFQ
{
    public interface IRFQService
    {
        Task<VmRfqPage> GetRfqByPage(int documentOwner, int page, int totalRecords);

        string SaveRFQ(VmRfq rfq);

        Task<VmRfq> GetSingleRfqById(int documentId);
    }
}
