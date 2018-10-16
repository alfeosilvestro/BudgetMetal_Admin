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

        Task<VmCompanyPage> GetCompanySupplierList(string keyword, int page, int totalRecords);

        Task<VmCompanyItem> GetCompanyById(int Id);

        VmGenericServiceResult Insert(VmCompanyItem vmCodeTableItem);

        Task<VmGenericServiceResult> Update(VmCompanyItem codeTableItem);

        Task Delete(int Id);

        Task<VmCompanyPage> GetSupplierByServiceTagsId(string serviceTagsId, int page, string searchKeyword);

        Task<List<VmCompanyItem>> GetActiveCompanies();

        Task<VmCompanyItem> GetCompanyByUEN(string RegNo);

        Task<VmCompanyPage> GetSupplierByCompany(int companyId, int page, string keyword);

        Task<VmGenericServiceResult> EditCompanyAbout(int companyId, string about, string updatedBy);

        Task<VmGenericServiceResult> EditCompanyAddress(int companyId, string address, string updatedBy);
    }
}
