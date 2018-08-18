using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.DataRepository.EmailLog
{
    public class EmailLogRepository : GenericRepository<Com.BudgetMetal.DBEntities.EmailLog>, IEmailLogRepository
    {
        public EmailLogRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "EmailLogRepository")
        {

        }
    }
}
