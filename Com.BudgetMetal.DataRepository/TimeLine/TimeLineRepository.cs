using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.TimeLine
{
    public class TimeLineRepository : GenericRepository<Com.BudgetMetal.DBEntities.TimeLine>, ITimeLineRepository
    {
        public TimeLineRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "TimeLineRepository")
        {

        }

        
    }
}
