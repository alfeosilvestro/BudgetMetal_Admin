using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Com.BudgetMetal.DataRepository.SupplierServiceTags
{
    public class SupplierServiceTagsRepository : GenericRepository<Com.BudgetMetal.DBEntities.SupplierServiceTags>, ISupplierServiceTagsRepository
    {
        public SupplierServiceTagsRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "SupplierServiceTagsRepository")
        {
        }

        
    }
}
