using Com.BudgetMetal.DataRepository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Com.BudgetMetal.Common;

namespace Com.BudgetMetal.DataRepository.EmailLog
{
    public interface IEmailsLogRepository : IGenericRepository<Com.BudgetMetal.DBEntities.EmailLog>
    {
    }
}
