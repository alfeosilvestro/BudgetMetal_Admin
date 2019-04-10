using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.BudgetMetal.DataRepository.QuotationSupport
{
    public class QuotationSupportRepository : GenericRepository<Com.BudgetMetal.DBEntities.QuotationSupport>, IQuotationSupportRepository
    {
        public QuotationSupportRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "QuotationSupportRepository")
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
