using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.Company
{
    public interface ICompanyRepository : IGenericRepository<Com.BudgetMetal.DBEntities.Company>
    {
        PageResult<Com.BudgetMetal.DBEntities.Company> GetSupplierByServiceTagsId(string serviceTagsId, int page, int totalRecord);

        Task<PageResult<Com.BudgetMetal.DBEntities.Company>> GetSupplierList(string keyword, int page, int totalRecords);

        Task<Com.BudgetMetal.DBEntities.Company> GetCompanyByUEN(string RegNo);

        Task<PageResult<Com.BudgetMetal.DBEntities.Company>> GetSupplierByCompanyId(int companyId, int page, int totalRecords, string keyword);
    }
}
