using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DBEntities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.SupplierServiceTags
{
    public interface ISupplierServiceTagsRepository : IGenericRepository<Com.BudgetMetal.DBEntities.SupplierServiceTags>
    {
        Task<List<Com.BudgetMetal.DBEntities.SupplierServiceTags>> GetServiceTagByCompanyID(int companyId);
    }
}
