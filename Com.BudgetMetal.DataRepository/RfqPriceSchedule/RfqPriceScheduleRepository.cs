using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.BudgetMetal.DataRepository.RfqPriceSchedule
{
   
    public class RfqPriceScheduleRepository : GenericRepository<Com.BudgetMetal.DBEntities.RfqPriceSchedule>, IRfqPriceScheduleRepository
    {
        public RfqPriceScheduleRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "RfqPriceScheduleRepository")
        {

        }
        public void InactiveByRFQId(int rfqId, string UpdatedBy)
        {
            var dbResult = this.entities.Where(e => e.IsActive == true && e.Rfq_Id == rfqId).ToList();
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
