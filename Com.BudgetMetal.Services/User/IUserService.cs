using Com.BudgetMetal.Common;
using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.Sys_User;
using Com.BudgetMetal.ViewModels.User;
using System;
using System.Collections.Generic;
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

        Task<List<VmUserItem>> GetUserByCompany(int Id);

        Task<VmUserItem> ValidateUser(VM_Sys_User_Sign_In user);

        Task<bool> CheckEmail(string email);

        Task<bool> CheckUserName(string Username);

        Task<bool> CheckCurrentPassword(int id, string currentPssword);

        Task<VmGenericServiceResult> Register(VmUserItem user, string[] serviceTags);

        Task<VmGenericServiceResult> ConfirmUserName(string Username);

        Task<VmGenericServiceResult> ResetPassword(string username, string newPassword);

        Task<VmGenericServiceResult> ChangePassword(int id, string password);
    }
}
