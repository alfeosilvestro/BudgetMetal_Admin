using Com.BudgetMetal.Common;
using Com.BudgetMetal.ViewModels.User;
using System;

namespace Com.BudgetMetal.Services.Users
{
    public interface IUserService
    {
        VmUserPage GetUserByPage(string keyword, int page, int totalRecords);

        VmUserItem GetUserById(int Id);

        void Insert(VmUserItem vmUserItem);

        void Update(VmUserItem userItem);

        void Delete(int Id);
    }
}
