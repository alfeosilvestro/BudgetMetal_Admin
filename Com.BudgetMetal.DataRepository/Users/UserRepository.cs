using Com.BudgetMetal.Common;
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
            var records  = await entities.Where(e => e.IsActive == true && e.IsConfirmed == true
            && e.Company_Id == Id)
            .OrderBy(e => e.ContactName)
            .ToListAsync();

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
        //            //Title = r.Title,
        //            //RoleId = r.RoleId,
        //            //SiteAdmin = r.SiteAdmin,
        //            //UserTypeId = r.UserTypeId,
        //            //Email = r.Email,
        //            //Confirmed = r.Confirmed,
        //            //Status = r.Status
        //        })
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


    }
}
