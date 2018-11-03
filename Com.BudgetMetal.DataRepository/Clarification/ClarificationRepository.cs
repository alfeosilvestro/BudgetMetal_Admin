using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.Clarification
{
    public class ClarificationRepository : GenericRepository<Com.BudgetMetal.DBEntities.Clarification>, IClarificationRepository
    {
        public ClarificationRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "ClarificationRepository")
        {

        }

       
    }
}
