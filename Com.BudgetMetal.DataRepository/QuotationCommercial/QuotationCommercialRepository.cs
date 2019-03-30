using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.BudgetMetal.DataRepository.QuotationCommercial
{
    public class QuotationCommercialRepository : GenericRepository<Com.BudgetMetal.DBEntities.QuotationCommercial>, IQuotationCommercialRepository
    {
        public QuotationCommercialRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "QuotationCommercialRepository")
        {

        }

        public void InactiveByQuotationId(int quotationId, string UpdatedBy)
        {
            var dbResult = this.entities.Where(e => e.IsActive == true && e.Quotation_Id == quotationId).ToList();
            dbResult.ForEach(e =>
            {
                e.IsActive = false;
                e.UpdatedDate = DateTime.Now;
                e.UpdatedBy = UpdatedBy;
            }
            );
        }
    }
}
