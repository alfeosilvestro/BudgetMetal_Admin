using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.DataRepository.Sla
{
   
    public class SlaRepository : GenericRepository<Com.BudgetMetal.DBEntities.Sla>, ISlaRepository
    {
        public SlaRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "SlaRepository")
        {

        }

    }
}
