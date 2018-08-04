using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.DataRepository.ServiceTags
{
    public class ServiceTagsRepository : GenericRepository<Com.BudgetMetal.DBEntities.ServiceTags>, IServiceTagsRepository
    {
        public ServiceTagsRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "ServiceTagsRepository")
        {

        }

    }
}
