using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Com.BudgetMetal.DB.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.Roles
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "RoleRepository")
        {

        }

        public PageResult<Role> GetRolesByPage(string keyword, int page, int totalRecords)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                keyword = string.Empty;
                //return await base.GetPage(keyword, page, totalRecords);
            }

            var records = this.DbContext.Role.Where(e => 
                e.IsActive==true &&
                (keyword == string.Empty ||
                e.Name.Contains(keyword) ||
                e.Code.Contains(keyword))
            );

            var recordList = records
            .Select(r =>
                new Role()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Code = r.Code
                })
            .OrderBy(e => e.Name)
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

            var result = new PageResult<Role>()
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

        public Role GetRoleById(int Id)
        {
            var records = this.DbContext.Role.Where(x=>x.IsActive==true)
                .Single(e =>
                e.Id == Id);

            return records;
        }

        public Role GetRoleFileById(int Id)
        {
            var records = this.DbContext.Role.Select(r =>
                new Role()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Code = r.Code
                })
                .Single(e =>
                e.Id == Id);

            return records;
        }
    }
}
