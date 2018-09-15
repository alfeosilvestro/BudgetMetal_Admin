using Com.BudgetMetal.DataRepository.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.DataRepository.QuotationRequirement
{
    public interface IQuotationRequirementRepository : IGenericRepository<Com.BudgetMetal.DBEntities.QuotationRequirement>
    {
        void InactiveByQuotationId(int quotationId, string UpdatedBy);
    }
}
