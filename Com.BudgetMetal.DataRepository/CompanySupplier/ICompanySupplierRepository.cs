using Com.BudgetMetal.DataRepository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.CompanySupplier
{
    public interface ICompanySupplierRepository : IGenericRepository<Com.BudgetMetal.DBEntities.CompanySupplier>
    {
        void InactivePreferedCompanySupplier(int companyId, int supplierId, string updatedBy);

        bool IsExistedSupplier(int companyId, int supplierId);
        Task<List<int>> GetPreferredSupplierByCompanyId(int companyId);
    }
}
