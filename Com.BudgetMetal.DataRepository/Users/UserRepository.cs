using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Com.BudgetMetal.DB.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.Users
{
    public class UserRepository : GenericRepository<user>, IUserRepository
    {
        public UserRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "GalleryRepository")
        {

        }

        public PageResult<user> GetUsersByPage(string keyword, int page, int totalRecords)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                keyword = string.Empty;
                //return await base.GetPage(keyword, page, totalRecords);
            }

            var records = this.DbContext.user.Where(e => 
                e.IsActive==true &&
                (keyword == string.Empty ||
                e.UserName.Contains(keyword) ||
                e.Title.Contains(keyword))
            );

            var recordList = records
            .Select(r =>
                new user()
                {
                    Id = r.Id,
                    UserName = r.UserName,
                    Password = r.Password,
                    Title = r.Title,
                    RoleId = r.RoleId,
                    SiteAdmin = r.SiteAdmin,
                    UserTypeId = r.UserTypeId,
                    Email   = r.Email,
                    Confirmed = r.Confirmed,
                    Status = r.Status
                })
            .OrderBy(e => e.UserName)
            .OrderBy(e => e.CreatedDate)
            .Skip((totalRecords * page) - totalRecords)
            .Take(totalRecords)
            .ToList();
            //DetailImage = (getDetailImage ? r.DetailImage : null),

            var count = records.Count();

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

            var result = new PageResult<user>()
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

        public user GetUserById(int Id)
        {
            var records = this.DbContext.user.Where(x=>x.IsActive==true).Select(r =>
                new user()
                {
                    Id = r.Id,
                    UserName = r.UserName,
                    Password = r.Password,
                    Title = r.Title,
                    RoleId = r.RoleId,
                    SiteAdmin = r.SiteAdmin,
                    UserTypeId = r.UserTypeId,
                    Email = r.Email,
                    Confirmed = r.Confirmed,
                    Status = r.Status
                })
                .Single(e =>
                e.Id == Id);

            return records;
        }

        public roles GetRoleFileById(int Id)
        {
            var records = this.DbContext.roles.Select(r =>
                new roles()
                {
                    Id = r.Id,
                    Role = r.Role,
                    RoleCode = r.RoleCode
                })
                .Single(e =>
                e.Id == Id);

            return records;
        }

        
    }
}
