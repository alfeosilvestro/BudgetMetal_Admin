using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.DataRepository.Penalty
{
   
    public class PenaltyRepository : GenericRepository<Com.BudgetMetal.DBEntities.Penalty>, IPenaltyRepository
    {
        public PenaltyRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "PenaltyRepository")
        {

        }

    }
}
