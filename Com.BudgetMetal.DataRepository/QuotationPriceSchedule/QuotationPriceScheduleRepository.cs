using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Com.BudgetMetal.DBEntities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.QuotationPriceSchedule
{
    public class QuotationPriceScheduleRepository : GenericRepository<Com.BudgetMetal.DBEntities.QuotationPriceSchedule>, IQuotationPriceScheduleRepository
    {
        public QuotationPriceScheduleRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "QuotationPriceScheduleRepository")
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
