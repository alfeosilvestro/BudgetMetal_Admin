using Com.BudgetMetal.Common;
using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.Role;
using System;

namespace Com.BudgetMetal.Services.Roles
{
    public interface IRoleService
    {
        VmRolePage GetRolesByPage(string keyword, int page, int totalRecords);

        VmRoleItem GetRoleById(int Id);

        VmGenericServiceResult Insert(VmRoleItem vmRoleItem);

        VmGenericServiceResult Update(VmRoleItem roleItem);

        void Delete(int I);
    }
}
