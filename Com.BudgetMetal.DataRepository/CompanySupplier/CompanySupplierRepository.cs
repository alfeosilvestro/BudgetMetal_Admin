using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Com.BudgetMetal.DataRepository.CompanySupplier
{
    public class CompanySupplierRepository : GenericRepository<Com.BudgetMetal.DBEntities.CompanySupplier>, ICompanySupplierRepository
    {
        public CompanySupplierRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "CompanySupplierRepository")
        {
        }
    }
}
