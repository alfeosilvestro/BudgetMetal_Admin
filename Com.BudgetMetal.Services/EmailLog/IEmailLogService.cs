using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.EmailLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.Services.EmailLog
{
    public interface IEmailLogService
    {
        Task<VmEmailLogPage> GetEmailLogByPage(string keyword, int page, int totalRecords);

        Task<VmEmailLogItem> GetEmailLogById(int Id);

        VmGenericServiceResult Insert(VmEmailLogItem vmEmailLogItem);

        Task<VmGenericServiceResult> Update(VmEmailLogItem vmEmailLogItem);

        Task Delete(int Id);
    }
}
