﻿using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DBEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.Users
{
    public interface IUserRepository : IGenericRepository<User>
    {
        //PageResult<User> GetUsersByPage1(string keyword, int page, int totalRecords);

        //User GetUserById(int Id );

        //Role GetRoleFileById(int Id);
        Task<User> GetUser(string username, string password);

        Task<List<User>> GetUserByCompany(int Id);

        Task<Com.BudgetMetal.DBEntities.User> GetUserById(int id);

        Task<Com.BudgetMetal.DBEntities.User> GetUserByEmail(string email);

        Task<Com.BudgetMetal.DBEntities.User> GetUserByUserName(string UserName);

        Task<Com.BudgetMetal.DBEntities.User> GetUserByUserNameOrEmail(string UserName);

        List<string> GetSupplierAdmin(List<int> supplierList);

        List<User> GetSupplierAdminUser(int supplierId);

        Task<Com.BudgetMetal.DBEntities.User> GetUserCompanyIdandUserId(int companyId, int userId);

        Task<List<User>> GetUserByCompanyNotFilterWithConfirm(int Id);

        List<string> GetBuyerAdmin(int company_Id);

        List<User> GetBuyerAdminUser(int company_Id);
    }
}
