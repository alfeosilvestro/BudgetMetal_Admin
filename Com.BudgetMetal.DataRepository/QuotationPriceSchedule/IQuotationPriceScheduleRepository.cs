using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DBEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.QuotationPriceSchedule
{
    public interface IQuotationPriceScheduleRepository : IGenericRepository<Com.BudgetMetal.DBEntities.QuotationPriceSchedule>
    {
        void InactiveByQuotationId(int quotationId, string UpdatedBy);
    }
}
