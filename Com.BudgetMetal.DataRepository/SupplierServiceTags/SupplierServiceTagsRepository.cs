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

        public async Task<List<Com.BudgetMetal.DBEntities.SupplierServiceTags>> GetServiceTagByCompanyID(int companyId)
        {
            return await this.entities.Where(e => e.IsActive == true && e.Company_Id == companyId).ToListAsync();
        }
        
    }
}
