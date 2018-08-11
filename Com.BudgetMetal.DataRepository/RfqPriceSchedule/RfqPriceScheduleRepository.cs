using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.DataRepository.RfqPriceSchedule
{
   
    public class RfqPriceScheduleRepository : GenericRepository<Com.BudgetMetal.DBEntities.RfqPriceSchedule>, IRfqPriceScheduleRepository
    {
        public RfqPriceScheduleRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "RfqPriceScheduleRepository")
        {

        }

    }
}
