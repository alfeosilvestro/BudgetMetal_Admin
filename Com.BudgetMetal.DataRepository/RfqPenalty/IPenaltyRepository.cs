using Com.BudgetMetal.DataRepository.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.DataRepository.Penalty
{
    public interface IPenaltyRepository : IGenericRepository<Com.BudgetMetal.DBEntities.Penalty>
    {
        void InactiveByRFQId(int rfqId, string UpdatedBy);
    }
    
}
