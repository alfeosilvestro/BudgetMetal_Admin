using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.Company;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.Services.Company
{
    public interface ICompanyService
    {
        Task<VmCompanyPage> GetCompanyByPage(string keyword, int page, int totalRecords);

        Task<VmCompanyItem> GetCompanyById(int Id);

        VmGenericServiceResult Insert(VmCompanyItem vmCodeTableItem);

        Task<VmGenericServiceResult> Update(VmCompanyItem codeTableItem);

        Task Delete(int Id);
    }
}
