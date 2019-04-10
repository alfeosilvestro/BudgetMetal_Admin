using Com.BudgetMetal.DataRepository.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.DataRepository.QuotationSupport
{
    public interface IQuotationSupportRepository : IGenericRepository<Com.BudgetMetal.DBEntities.QuotationSupport>
    {
        void InactiveByQuotationId(int quotationId, string UpdatedBy);
    }
}
