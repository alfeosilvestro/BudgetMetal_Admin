using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Com.BudgetMetal.DBEntities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Com.BudgetMetal.Common;

namespace Com.BudgetMetal.DataRepository.Industries
{
    public class IndustryRepository : GenericRepository<Industry>, IIndustryRepository
    {
        public IndustryRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "IndustryRepository")
        {

        }
       
    }
}
