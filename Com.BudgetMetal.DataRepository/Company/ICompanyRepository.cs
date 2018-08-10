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
    }
}
