using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.DataRepository.RFQ
{
   
    public class RfqRepository : GenericRepository<Com.BudgetMetal.DBEntities.Rfq>, IRfqRepository
    {
        public RfqRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "RfqRepository")
        {

        }

    }
}
