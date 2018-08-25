using Com.BudgetMetal.DataRepository.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.DataRepository.RfqPriceSchedule
{
    public interface IRfqPriceScheduleRepository : IGenericRepository<Com.BudgetMetal.DBEntities.RfqPriceSchedule>
    {
        void InactiveByRFQId(int rfqId, string UpdatedBy);
    }
    
}
