using Com.BudgetMetal.DataRepository.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.DataRepository.QuotationCommercial
{
    public interface IQuotationCommercialRepository : IGenericRepository<Com.BudgetMetal.DBEntities.QuotationCommercial>
    {
        void InactiveByQuotationId(int quotationId, string UpdatedBy);
    }
}
