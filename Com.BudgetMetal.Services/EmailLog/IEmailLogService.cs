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
    }
}
