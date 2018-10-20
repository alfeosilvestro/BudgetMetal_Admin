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
        public async Task<List<int>> GetPreferredSupplierByCompanyId(int companyId)
        {
            var resultList = new List<int>();

            resultList = await this.entities.Where(e => e.IsActive == true && e.Company_Id == companyId).Select(e => e.Supplier_Id).Distinct().ToListAsync();

            return resultList;
        }
    }
}
