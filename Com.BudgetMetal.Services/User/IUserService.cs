using Com.BudgetMetal.Common;
using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.User;
using System;
using System.Threading.Tasks;

namespace Com.BudgetMetal.Services.Users
{
    public interface IUserService
    {
        Task<VmUserPage> GetUserByPage(string keyword, int page, int totalRecords);

        Task<VmUserItem> GetUserById(int Id);

        Task<VmGenericServiceResult> Insert(VmUserItem vmUserItem);

        Task<VmGenericServiceResult> Update(VmUserItem vmUserItem);

        Task Delete(int Id);

        Task<VmUserItem> GetFormObject();
    }
}
