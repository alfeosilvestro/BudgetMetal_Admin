using Com.BudgetMetal.Common;
using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.Role;
using System;
using System.Threading.Tasks;

namespace Com.BudgetMetal.Services.Roles
{
    public interface IRoleService
    {
        Task<VmRolePage> GetRolesByPage(string keyword, int page, int totalRecords);

        Task<VmRoleItem> GetRoleById(int Id);

        VmGenericServiceResult Insert(VmRoleItem vmRoleItem);

        Task<VmGenericServiceResult> Update(VmRoleItem roleItem);

        Task Delete(int I);
    }
}
